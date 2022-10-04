using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Suinet.Rpc;
using Suinet.Rpc.Types;
using UnityEngine;

public class LocalPlayerTile : MonoBehaviour
{
    public float moveSpeed = 6.0f;

    //private string _onChainStateObjectId = "";

    private Rigidbody2D rb;
    private IJsonRpcApiClient _fullNodeClient;
    private IJsonRpcApiClient _gatewayClient;
    private const int UpdatePeriodInMs = 1000;
    private Vector2 _lastPosition = Vector2.zero;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _fullNodeClient = new SuiJsonRpcApiClient(new UnityWebRequestRpcClient(SuiConstants.DEVNET_FULLNODE_ENDPOINT));
        _gatewayClient = new SuiJsonRpcApiClient(new UnityWebRequestRpcClient(SuiConstants.DEVNET_GATEWAY_ENDPOINT));
        
        // if (PlayerPrefs.HasKey(Constants.ONCHAIN_STATE_OBJECT_ID_KEY)) 
        // { 
        //     _onChainStateObjectId = PlayerPrefs.GetString(Constants.ONCHAIN_STATE_OBJECT_ID_KEY); 
        // } 
        //
        rb.velocity = Vector2.up * moveSpeed;
        StartCoroutine(UpdateOnChainPlayerStateWorker());
    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0.0f)
        {
            var dir = Input.GetAxis("Horizontal") > 0.0f ? 1.0f : -1.0f;
             rb.velocity = Quaternion.Euler(0, 0, 90 * dir ) * rb.velocity.normalized;
             rb.velocity *= moveSpeed;
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
    
    private IEnumerator UpdateOnChainPlayerStateWorker() 
    { 
        while (true)
        {
            var task = UpdateOnChainPlayerStateAsync(rb.position, rb.velocity);
            yield return new WaitUntil(()=> task.IsCompleted);
        }
    }
    
    private async Task UpdateOnChainPlayerStateAsync(Vector2 position, Vector2 velocity)
    {
        //if (_ongoingRequestIds.Count > 0) return;
        
        //Debug.Log("DoMoveOnChainAsync");
        
        // var requestId = Guid.NewGuid().ToString();
        // _ongoingRequestIds.Add(requestId);
        
       // Debug.Log("UpdateOnChainPlayerStateAsync");

       if (_lastPosition == position)
       {
           return;
       }
       
       var signer = SuiWallet.GetActiveAddress();

        if (string.IsNullOrWhiteSpace(signer))
        {
            Debug.Log("signer is null UpdateOnChainPlayerStateAsync early return");
            return;
        }

        var onChainStateObjectId = "";
        if (PlayerPrefs.HasKey(Constants.ONCHAIN_STATE_OBJECT_ID_KEY))
        {
            onChainStateObjectId = PlayerPrefs.GetString(Constants.ONCHAIN_STATE_OBJECT_ID_KEY);
        }
        if (string.IsNullOrWhiteSpace(onChainStateObjectId))
        {
            Debug.Log("onChainStateObjectId is null UpdateOnChainPlayerStateAsync early return");
            return;
        }

        var packageObjectId = Constants.PACKAGE_OBJECT_ID; 
        var module = "movement2_module"; 
        var function = "do_update"; 
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

        //Debug.Log("gasobjectid: " + gasObjectId);

        var onChainPosition = new OnChainVector2(position);
        var onChainVelocity = new OnChainVector2(velocity);
        
        var args = new object[] { onChainStateObjectId, onChainPosition.x, onChainPosition.y, onChainVelocity.x, onChainVelocity.y };
        var rpcResult = await _gatewayClient.MoveCallAsync(signer, packageObjectId, module, function, typeArgs, args, gasObjectId, 2000); 
 
        //_ongoingRequestIds.Remove(requestId);

        //Debug.Log(JsonConvert.SerializeObject(rpcResult));
        
        if (rpcResult.IsSuccess) 
        { 
            var txBytes = rpcResult.Result.TxBytes;
            await ExecuteTransactionAsync(txBytes);
        } 
        else 
        { 
            Debug.LogError("Something went wrong with the move call: " + rpcResult.ErrorMessage);
        }
        
        _lastPosition = position;
        // _ongoingRequestIds.Remove(requestId);
    }
}
