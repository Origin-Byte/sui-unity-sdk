using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Suinet.Rpc;
using Suinet.Rpc.Http;
using Suinet.Rpc.Types;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OnChainGameController : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase playerTile;
    
    private string _positionObjectId = "";
    private Timer _updateTimer;
    private Timer _drawTimer;
    private const string PACKAGE_OBJECT_ID = "0xbcf4c404963ef0ae205bdecd80722543948cbb94";

    private IJsonRpcApiClient _fullNodeClient;
    private IJsonRpcApiClient _gatewayClient;

    private Dictionary<string, Tuple<UInt64, UInt64>> _playerPositions;

    private ulong _latestEventReadTimeStamp = 0;
    
    // Start is called before the first frame update
    async void Start()
    {
        _playerPositions = new Dictionary<string, Tuple<ulong, ulong>>();
        _fullNodeClient = new SuiJsonRpcApiClient(new UnityWebRequestRpcClient(SuiConstants.DEVNET_FULLNODE_ENDPOINT));
        _gatewayClient = new SuiJsonRpcApiClient(new UnityWebRequestRpcClient(SuiConstants.DEVNET_GATEWAY_ENDPOINT));
        
        if (PlayerPrefs.HasKey("positionObject")) 
        { 
            _positionObjectId = PlayerPrefs.GetString("positionObject"); 
        } 
        
        if (string.IsNullOrWhiteSpace(_positionObjectId))
        { 
            _positionObjectId = await CreatePositionAsync(); 
            PlayerPrefs.SetString("positionObject", _positionObjectId); 
        } 
        
        print("positionObjectId: " + _positionObjectId);

        _updateTimer = new System.Timers.Timer(500); 
        _updateTimer.Elapsed += UpdateTimerElapsed; 
        _updateTimer.AutoReset = true; 
        _updateTimer.Enabled = true; 
        
        _drawTimer = new System.Timers.Timer(250);
        _drawTimer.Elapsed += DrawTimerElapsed; 
        _drawTimer.Enabled = true;
        _drawTimer.AutoReset = true;
    }

    void OnDestroy()
    {
        _updateTimer.Stop();
        _updateTimer.Elapsed -= UpdateTimerElapsed;
        _drawTimer.Stop();
        _drawTimer.Elapsed -= DrawTimerElapsed;
    }

    private void DrawTimerElapsed(object sender, ElapsedEventArgs e)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(Draw);
    }
    
    private void UpdateTimerElapsed(object sender, ElapsedEventArgs e) 
    { 
        UnityMainThreadDispatcher.Instance().Enqueue(async () => await ReadInputAsync());
        UnityMainThreadDispatcher.Instance().Enqueue(async () => await GetMovementEventsAsync());
    }

    private void Draw()
    {
        tilemap.ClearAllTiles();
        foreach (var position in _playerPositions)
        {

            // map from uint storage format
            var x = Convert.ToInt32(position.Value.Item1) - 100000;
            var y = Convert.ToInt32(position.Value.Item2) - 100000;
            var pos = new Vector3Int(x, y);
            Debug.Log("_playerPositions x " + x + ", y: " + y);

            tilemap.SetTile(pos, playerTile);
        }
    }
    
    private async Task ReadInputAsync()
    {
        Debug.Log("tilemap size: " + tilemap.size);
        
        var normalizedHorizontal = Mathf.RoundToInt(Input.GetAxis("Horizontal")) + 1;
        var normalizedVertical = Mathf.RoundToInt(Input.GetAxis("Vertical")) + 1;

        if (normalizedHorizontal != 1 || normalizedVertical != 1)
        {
             await DoMoveOnChainAsync(normalizedHorizontal, normalizedVertical);
        }
    }

    private async Task GetMovementEventsAsync()
    {
        var rpcResult = await _fullNodeClient.GetEventsByModuleAsync(PACKAGE_OBJECT_ID, "movement_module", 10, _latestEventReadTimeStamp, 10000000000000 );
        if (rpcResult.IsSuccess)
        {
            var movementEvents = JArray.FromObject(rpcResult.Result);

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
                    var x = BitConverter.ToUInt64(bytes, 0);
                    var y = BitConverter.ToUInt64(bytes, 8);

                    if (_playerPositions.ContainsKey(sender))
                    {
                        _playerPositions[sender] = new Tuple<ulong, ulong>(x, y);
                    }
                    else
                    {
                        _playerPositions.Add(sender, new Tuple<ulong, ulong>(x, y));
                    }
                }
            }
        }
    }
    
    private async Task DoMoveOnChainAsync(int x, int y)
    {
        var signer = SuiWallet.GetActiveAddress(); 
        var packageObjectId = PACKAGE_OBJECT_ID; 
        var module = "movement_module"; 
        var function = "do_move"; 
        var typeArgs = System.Array.Empty<string>(); 
 
        var gasObjectId = ""; 
        if (PlayerPrefs.HasKey("gasObjectId")) 
        { 
            gasObjectId = PlayerPrefs.GetString("gasObjectId"); 
        } 
        else 
        { 
            gasObjectId = (await SuiHelper.GetCoinObjectIdsAboveBalancesOwnedByAddressAsync(signer, 1, 10000))[0]; 
            PlayerPrefs.SetString("gasObjectId", gasObjectId); 
        } 
 
        var args = new object[] { _positionObjectId, x, y };
        var rpcResult = await _gatewayClient.MoveCallAsync(signer, packageObjectId, module, function, typeArgs, args, gasObjectId, 2000); 
 
        if (rpcResult.IsSuccess) 
        { 
            var keyPair = SuiWallet.GetActiveKeyPair(); 
 
            var txBytes = rpcResult.Result.TxBytes; 
            var signature = keyPair.Sign(rpcResult.Result.TxBytes); 
            var pkBase64 = keyPair.PublicKeyBase64; 
 
            var txRpcResult = await _gatewayClient.ExecuteTransactionAsync(txBytes, SuiSignatureScheme.ED25519, signature, pkBase64); 
            if (txRpcResult.IsSuccess) 
            { 
               // var txEffects = JObject.FromObject(txRpcResult.Result.Effects); 
            } 
            else 
            { 
                Debug.LogError("Something went wrong when executing the transaction: " + txRpcResult.ErrorMessage); 
            } 
        } 
        else 
        { 
            Debug.LogError("Something went wrong with the move call: " + rpcResult.ErrorMessage); 
        } 
    }
    
    private async Task<string> CreatePositionAsync() 
    { 
        var signer = SuiWallet.GetActiveAddress(); 
        var packageObjectId = PACKAGE_OBJECT_ID; 
        var module = "movement_module"; 
        var function = "create_position_for_sender"; 
        var typeArgs = System.Array.Empty<string>(); 
 
        var gasObjectId = ""; 
        if (PlayerPrefs.HasKey("gasObjectId")) 
        { 
            gasObjectId = PlayerPrefs.GetString("gasObjectId"); 
        } 
        else 
        { 
            gasObjectId = (await SuiHelper.GetCoinObjectIdsAboveBalancesOwnedByAddressAsync(signer, 1, 10000))[0]; 
            PlayerPrefs.SetString("gasObjectId", gasObjectId); 
        } 
 
        var args = System.Array.Empty<object>(); 
        var rpcResult = await _gatewayClient.MoveCallAsync(signer, packageObjectId, module, function, typeArgs, args, gasObjectId, 2000); 
 
        var createdObjectId = ""; 
 
        if (rpcResult.IsSuccess) 
        { 
            var keyPair = SuiWallet.GetActiveKeyPair(); 
 
            var txBytes = rpcResult.Result.TxBytes; 
            var signature = keyPair.Sign(rpcResult.Result.TxBytes); 
            var pkBase64 = keyPair.PublicKeyBase64; 
 
            var txRpcResult = await _gatewayClient.ExecuteTransactionAsync(txBytes, SuiSignatureScheme.ED25519, signature, pkBase64); 
            if (txRpcResult.IsSuccess) 
            { 
                var txEffects = JObject.FromObject(txRpcResult.Result.Effects); 
 
                createdObjectId = txEffects.SelectToken("created[0].reference.objectId").Value<string>(); 
 
            } 
            else 
            { 
                Debug.LogError("Something went wrong when executing the transaction: " + txRpcResult.ErrorMessage); 
            } 
        } 
        else 
        { 
            Debug.LogError("Something went wrong with the move call: " + rpcResult.ErrorMessage); 
        } 
 
        return createdObjectId; 
    }
    
    private void DrawTiles()
    {
        foreach (var VARIABLE in name)
        {
            
        }
    }
}
