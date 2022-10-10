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
    public static OnChainStateStore Instance { get; private set; }
    public readonly Dictionary<string, OnChainPlayerState> States = new Dictionary<string, OnChainPlayerState>();
    public Transform playersParent;
    public Transform explosionsParent;
    public Transform trailCollidersParent;
    public OnChainPlayer remotePlayerPrefab;
    public TrailCollider trailColliderPrefab;
    
    private readonly Dictionary<string, OnChainPlayer> _remotePlayers = new Dictionary<string, OnChainPlayer>();
    private ulong _latestEventReadTimeStamp;
    private IJsonRpcApiClient _fullNodeClient;
    private IJsonRpcApiClient _gatewayClient;
    private string _localPlayerAddress;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // start reading events from 1 second ago
       // _latestEventReadTimeStamp = Convert.ToUInt64(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - 1000);
        _latestEventReadTimeStamp = 0;
        
        _fullNodeClient = new SuiJsonRpcApiClient(new UnityWebRequestRpcClient(SuiConstants.DEVNET_FULLNODE_ENDPOINT));
        _gatewayClient = new SuiJsonRpcApiClient(new UnityWebRequestRpcClient(SuiConstants.DEVNET_GATEWAY_ENDPOINT));

        SetLocalPlayerAddress();
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

    private void SetLocalPlayerAddress()
    {
        if (string.IsNullOrWhiteSpace(_localPlayerAddress))
        {
            _localPlayerAddress = SuiWallet.GetActiveAddress();
        }
        if (string.IsNullOrWhiteSpace(_localPlayerAddress))
        {
            Debug.LogError("Could not retrieve active Sui address");
        }
    }
    
    private async Task GetOnChainUpdateEventsAsync()
    {
        SetLocalPlayerAddress();
        var rpcResult = await _fullNodeClient.GetEventsByModuleAsync(Constants.PACKAGE_OBJECT_ID, Constants.MODULE_NAME, 10, 0, 10000000000000 );
        if (rpcResult.IsSuccess)
        {
            var eventsArray = JArray.FromObject(rpcResult.Result);
            Debug.Log("GetOnChainUpdateEventsAsync: " + JsonConvert.SerializeObject(eventsArray));
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
                    var isExploded = BitConverter.ToBoolean(bytes, 40);

                    var position = new OnChainVector2(posX64, posY64);
                    var velocity = new OnChainVector2(velX64, velY64);

                    var state = new OnChainPlayerState(position, velocity, sequenceNumber, isExploded);

                    bool isLocalSender = sender == _localPlayerAddress;
                    
                    if (States.ContainsKey(sender)) 
                    {
                        if (sequenceNumber > States[sender].SequenceNumber || sequenceNumber == 0)
                        {
                            States[sender] = state;
                        }
                    }
                    else
                    {
                        if ((sequenceNumber != 0 || !isLocalSender) && !isExploded)
                        {
                            States.Add(sender, state);
                        }
                    }
                   // Debug.Log($"DrawSphere: {position.ToVector3()}. sequenceNumber: {sequenceNumber}");
                  //  GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                  //  cube.transform.position = position.ToVector3() + Vector3.back;
                }
            }
        }

        UpdateRemotePlayers();
    }

    private void UpdateRemotePlayers()
    {
        foreach (var state in States)
        {
            if (state.Key != _localPlayerAddress)
            {
                if (!_remotePlayers.ContainsKey(state.Key))
                {
                    var remotePlayerGo = Instantiate(remotePlayerPrefab, playersParent);
                    remotePlayerGo.ownerAddress = state.Key;
                    remotePlayerGo.GetComponent<ExplosionController>().explosionRoot = explosionsParent;
                    remotePlayerGo.gameObject.SetActive(true);
                    _remotePlayers.Add(state.Key, remotePlayerGo);
                    
                    var trailColliderGo =  Instantiate(trailColliderPrefab, trailCollidersParent);
                    trailColliderGo.ownerAddress = state.Key;
                    trailColliderGo.gameObject.SetActive(true);
                }
            }
        }
    }

    public void RemoveRemotePlayer(string address)
    {
        States.Remove(address);
        _remotePlayers.Remove(address);
    }
}
