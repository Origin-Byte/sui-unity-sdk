using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Suinet.Rpc;
using Suinet.Rpc.Types;
using UnityEngine;

public class OnChainStateStore : MonoBehaviour
{
    public static Dictionary<string, OnChainPlayerState> States = new Dictionary<string, OnChainPlayerState>();
    private ulong _latestEventReadTimeStamp = 0;
    private IJsonRpcApiClient _fullNodeClient;
    private IJsonRpcApiClient _gatewayClient;

    public Transform _playersParent;
    public OnChainPlayer remotePlayerPrefab;
    private Dictionary<string, OnChainPlayer> _remotePlayers = new Dictionary<string, OnChainPlayer>();

    private const int UpdatePeriodInMs = 3000;
    
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
            yield return new WaitUntil(()=> task.IsCompleted);
        }
    }
    
    private async Task GetOnChainUpdateEventsAsync()
    {
        var rpcResult = await _fullNodeClient.GetEventsByModuleAsync(Constants.PACKAGE_OBJECT_ID, "movement2_module", 10, _latestEventReadTimeStamp + 1, 10000000000000 );
        if (rpcResult.IsSuccess)
        {
            var eventsArray = JArray.FromObject(rpcResult.Result);
            foreach (var movementEvent in eventsArray)
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
                    
                    // BCS conversion
                    var bytes = Convert.FromBase64String(bcs);
                    var posX64 = BitConverter.ToUInt64(bytes, 0);
                    var posY64 = BitConverter.ToUInt64(bytes, 8);
                    var velX64 = BitConverter.ToUInt64(bytes, 16);
                    var velY64 = BitConverter.ToUInt64(bytes, 24);
                    var sequenceNumber = BitConverter.ToUInt64(bytes, 32);
                    
                    var position = new OnChainVector2(posX64, posY64);
                    var velocity = new OnChainVector2(velX64, velY64);

                    var state = new OnChainPlayerState(position, velocity, sequenceNumber);
                    
                    if (States.ContainsKey(sender)) 
                    {
                        if (sequenceNumber > States[sender].SequenceNumber || sequenceNumber == 0)
                        {
                            States[sender] = state;
                        }
                    }
                    else
                    {
                        States.Add(sender, state);
                    }
                }
            }
        }

        UpdateRemotePlayers();
    }

    private void UpdateRemotePlayers()
    {
        var localPlayerAddress = SuiWallet.GetActiveAddress();
        foreach (var state in States)
        {
            if (state.Key != localPlayerAddress)
            {
                if (!_remotePlayers.ContainsKey(state.Key))
                {
                    var remotePlayerGo = Instantiate(remotePlayerPrefab, _playersParent);
                    remotePlayerGo.ownerAddress = state.Key;
                    remotePlayerGo.gameObject.SetActive(true);
                    _remotePlayers.Add(state.Key, remotePlayerGo);
                }
            }
        }
    }
}
