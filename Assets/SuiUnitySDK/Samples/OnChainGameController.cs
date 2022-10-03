using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Suinet.Rpc;
using Suinet.Rpc.Http;
using Suinet.Rpc.Types;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OnChainGameController : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase playerTile;
    
    private string _positionObjectId = "";
    private const string PACKAGE_OBJECT_ID = "0xbcf4c404963ef0ae205bdecd80722543948cbb94";

    private IJsonRpcApiClient _fullNodeClient;
    private IJsonRpcApiClient _gatewayClient;

    private Dictionary<string, Vector3Int> _onChainPlayerPositions;
    private Dictionary<string, Vector3Int> _playerPositions;

    private HashSet<string> _ongoingRequestIds;

    private ulong _latestEventReadTimeStamp = 0;
    
    // Start is called before the first frame update
    async void Start()
    {
        _onChainPlayerPositions = new Dictionary<string, Vector3Int>();
        _playerPositions = new Dictionary<string, Vector3Int>();
        _fullNodeClient = new SuiJsonRpcApiClient(new UnityWebRequestRpcClient(SuiConstants.DEVNET_FULLNODE_ENDPOINT));
        _gatewayClient = new SuiJsonRpcApiClient(new UnityWebRequestRpcClient(SuiConstants.DEVNET_GATEWAY_ENDPOINT));

        _ongoingRequestIds = new HashSet<string>();
        
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

        StartCoroutine(ReadInputWorker());
        StartCoroutine(GetMovementEventsWorker());
        StartCoroutine(DrawWorker());
    }

    private IEnumerator DrawWorker()
    {
        while (true)
        {
            Draw();
            yield return new WaitForSeconds(0.15f);
        }
    }
    
    private IEnumerator ReadInputWorker() 
    { 
        while (true)
        {
            var task = ReadInputAsync();
            yield return new WaitUntil(()=> task.IsCompleted);
            //yield return new WaitForSeconds(0.3f);
        }
    }
    
    private IEnumerator GetMovementEventsWorker() 
    { 
        while (true)
        {
            var task = GetMovementEventsAsync();
            yield return new WaitUntil(()=> task.IsCompleted);
            //yield return new WaitForSeconds(0.3f);
        }
    }

    private IEnumerator ExecuteTransactionWorker() 
    {
        {
            var task = GetMovementEventsAsync();
            yield return new WaitUntil(()=> task.IsCompleted);
            //yield return new WaitForSeconds(0.3f);
        }
    }

    private async Task ExecuteTransactionAsync(string txBytes)
    {
        var keyPair = SuiWallet.GetActiveKeyPair(); 
 
        var signature = keyPair.Sign(txBytes); 
        var pkBase64 = keyPair.PublicKeyBase64; 
 
        var txRpcResult = await _fullNodeClient.ExecuteTransactionAsync(txBytes, SuiSignatureScheme.ED25519, signature, pkBase64); 
        if (txRpcResult.IsSuccess) 
        { 
            // var txEffects = JObject.FromObject(txRpcResult.Result.Effects); 
        } 
        else 
        { 
            Debug.LogError("Something went wrong when executing the transaction: " + txRpcResult.ErrorMessage);
        } 
    }
    
    private void Draw()
    {
        foreach (var position in _playerPositions)
        {
            if (_onChainPlayerPositions.ContainsKey(position.Key)
                && _onChainPlayerPositions[position.Key] != _playerPositions[position.Key])
            {
                tilemap.SetTile(_playerPositions[position.Key], null);
            }
        }

        _playerPositions.Clear();

        foreach (var position in _onChainPlayerPositions)
        {
            tilemap.SetTile(position.Value, playerTile);
            _playerPositions.Add(position.Key, position.Value);
        }
    }
    
    private async Task ReadInputAsync()
    {
       // Debug.Log("ReadInputAsync");
        
        var normalizedHorizontal = Mathf.RoundToInt(Input.GetAxis("Horizontal")) + 1;
        var normalizedVertical = Mathf.RoundToInt(Input.GetAxis("Vertical")) + 1;

        if (normalizedHorizontal != 1 || normalizedVertical != 1)
        {
             await DoMoveOnChainAsync(normalizedHorizontal, normalizedVertical);
        }
    }

    private async Task GetMovementEventsAsync()
    {
       // Debug.Log("GetMovementEventsAsync 0");

        var rpcResult = await _fullNodeClient.GetEventsByModuleAsync(PACKAGE_OBJECT_ID, "movement_module", 10, _latestEventReadTimeStamp, 10000000000000 );
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
                    var x64 = BitConverter.ToUInt64(bytes, 0);
                    var y64 = BitConverter.ToUInt64(bytes, 8);

                    // map from uint storage format
                    var x = Convert.ToInt32(x64) - 100000;
                    var y = Convert.ToInt32(y64) - 100000;
                    var pos = new Vector3Int(x, y);

                    if (_onChainPlayerPositions.ContainsKey(sender)) 
                    {
                        _onChainPlayerPositions[sender] = pos;
                    }
                    else
                    {
                        _onChainPlayerPositions.Add(sender, pos);
                    }
                }
            }
        }
    }
    
    private async Task DoMoveOnChainAsync(int x, int y)
    {
        //if (_ongoingRequestIds.Count > 0) return;
        
        //Debug.Log("DoMoveOnChainAsync");
        
       // var requestId = Guid.NewGuid().ToString();
       // _ongoingRequestIds.Add(requestId);
        
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
 
        //_ongoingRequestIds.Remove(requestId);

        if (rpcResult.IsSuccess) 
        { 
            var txBytes = rpcResult.Result.TxBytes;
            ExecuteTransactionAsync(txBytes); // intentionally not awaiting
        } 
        else 
        { 
            Debug.LogError("Something went wrong with the move call: " + rpcResult.ErrorMessage);
        }
       // _ongoingRequestIds.Remove(requestId);
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
