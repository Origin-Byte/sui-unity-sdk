using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Suinet.Rpc;
using Suinet.Rpc.Types;
using UnityEngine;

public class OnChainStateStore : MonoBehaviour
{
    public static Dictionary<string, OnChainPlayerState> States = new Dictionary<string, OnChainPlayerState>();
    private ulong _latestEventReadTimeStamp = 0;
    private const string PACKAGE_OBJECT_ID = "0xddd7d5858fabb04ea48da290d9f9031b7bd645e3";
    private IJsonRpcApiClient _fullNodeClient;
    private IJsonRpcApiClient _gatewayClient;

    private const int UpdatePeriodInMs = 300;
    
    static OnChainStateStore()
    {}

    private void Start()
    {
        _fullNodeClient = new SuiJsonRpcApiClient(new UnityWebRequestRpcClient(SuiConstants.DEVNET_FULLNODE_ENDPOINT));
        _gatewayClient = new SuiJsonRpcApiClient(new UnityWebRequestRpcClient(SuiConstants.DEVNET_GATEWAY_ENDPOINT));
        
        StartCoroutine(GetOnChainUpdateEventsWorker());
    }
    
    private IEnumerator GetOnChainUpdateEventsWorker() 
    { 
        while (true)
        {
            var task = GetOnChainUpdateEventsAsync();
            var waitTask = Task.Delay(UpdatePeriodInMs);
            yield return new WaitUntil(()=> task.IsCompleted && waitTask.IsCompleted);
        }
    }
    
    private async Task GetOnChainUpdateEventsAsync()
    {
        // Debug.Log("GetMovementEventsAsync 0");

        var rpcResult = await _fullNodeClient.GetEventsByModuleAsync(PACKAGE_OBJECT_ID, "movement2_module", 10, _latestEventReadTimeStamp, 10000000000000 );
        if (rpcResult.IsSuccess)
        {
            var movementEvents = JArray.FromObject(rpcResult.Result);
            //   Debug.Log("GetMovementEventsAsync success");

            foreach (var movementEvent in movementEvents)
            {
                if (movementEvent.SelectToken("Event.moveEvent") != null)
                {
                    var sender = movementEvent.SelectToken("Event.moveEvent.sender").Value<string>();
                    var bcs = movementEvent.SelectToken("Event.moveEvent.bcs").Value<string>();
                    var timeStamp = movementEvent.SelectToken("Timestamp").Value<ulong>();

                    if (timeStamp > _latestEventReadTimeStamp)
                    {
                        _latestEventReadTimeStamp = timeStamp;
                    }

                    byte[] bytes = Convert.FromBase64String(bcs);
                    var posX64 = BitConverter.ToUInt64(bytes, 0);
                    var posY64 = BitConverter.ToUInt64(bytes, 8);
                    var velX64 = BitConverter.ToUInt64(bytes, 16);
                    var velY64 = BitConverter.ToUInt64(bytes, 24);

                    var position = new OnChainPlayerState.OnChainVector2(posX64, posY64);
                    var velocity = new OnChainPlayerState.OnChainVector2(velX64, velY64);

                    var state = new OnChainPlayerState(position, velocity);
                    
                    if (States.ContainsKey(sender)) 
                    {
                        States[sender] = state;
                    }
                    else
                    {
                        States.Add(sender, state);
                    }
                }
            }
        }
    }
}
