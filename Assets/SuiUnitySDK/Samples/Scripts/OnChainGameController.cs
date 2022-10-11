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
    
    public static string onChainStateObjectId = "";

    private IJsonRpcApiClient _fullNodeClient;
    private IJsonRpcApiClient _gatewayClient;

    private Dictionary<string, Vector3Int> _playerPositions;

    private HashSet<string> _ongoingRequestIds;

    
    // Start is called before the first frame update
    async void Awake()
    {
        _playerPositions = new Dictionary<string, Vector3Int>();
        _fullNodeClient = new SuiJsonRpcApiClient(new UnityWebRequestRpcClient(SuiConstants.DEVNET_FULLNODE_ENDPOINT));
        _gatewayClient = new SuiJsonRpcApiClient(new UnityWebRequestRpcClient(SuiConstants.DEVNET_GATEWAY_ENDPOINT));

        _ongoingRequestIds = new HashSet<string>();

        if (PlayerPrefs.HasKey(Constants.ONCHAIN_STATE_OBJECT_ID_KEY)) 
        { 
            onChainStateObjectId = PlayerPrefs.GetString(Constants.ONCHAIN_STATE_OBJECT_ID_KEY); 
            Debug.Log("OnChainGameController.Awake onChainStateObjectId: " + onChainStateObjectId);
        } 
        
        if (string.IsNullOrWhiteSpace(onChainStateObjectId))
        { 
            onChainStateObjectId = await CreateOnChainPlayerStateAsync(); 
            PlayerPrefs.SetString(Constants.ONCHAIN_STATE_OBJECT_ID_KEY, onChainStateObjectId); 
        } 
        
        print("ONCHAIN_STATE_OBJECT_ID: " + onChainStateObjectId);

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
    

    private async Task ReadInputAsync()
    {
       // Debug.Log("ReadInputAsync");
        
        // var normalizedHorizontal = Mathf.RoundToInt(Input.GetAxis("Horizontal")) + 1;
        // var normalizedVertical = Mathf.RoundToInt(Input.GetAxis("Vertical")) + 1;
        //
        // if (normalizedHorizontal != 1 || normalizedVertical != 1)
        // {
        //      await DoMoveOnChainAsync(normalizedHorizontal, normalizedVertical);
        // }
    }

    private async Task<string> CreateOnChainPlayerStateAsync() 
    { 
        var signer = SuiWallet.GetActiveAddress(); 
        var packageObjectId = Constants.PACKAGE_OBJECT_ID;
        var module = Constants.MODULE_NAME; 
        var function = "create_playerstate_for_sender"; 
        var typeArgs = System.Array.Empty<string>(); 
 
        Debug.Log("CreateOnChainPlayerStateAsync");
        
        var gasObjectId = ""; 
        if (PlayerPrefs.HasKey("gasObjectId")) 
        { 
            gasObjectId = PlayerPrefs.GetString("gasObjectId"); 
        } 
        else
        {
            var objects = (await SuiHelper.GetCoinObjectIdsAboveBalancesOwnedByAddressAsync(_gatewayClient, signer, 1, 10000));
            if (objects.Count > 0)
            {
                gasObjectId =  objects[0];
                PlayerPrefs.SetString("gasObjectId", gasObjectId);

            }
            else
            {
                Debug.LogError("could not retrieve coin objects!");
                return "";
            }
        }

        var args = new object[] { TimestampService.UtcTimestamp };
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
                Debug.Log("CreatedOnChainPlayerStateAsync. createdObjectId: " + createdObjectId);
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
}
