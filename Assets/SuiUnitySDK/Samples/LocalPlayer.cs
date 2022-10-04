using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Suinet.Rpc;
using Suinet.Rpc.Types;
using UnityEngine;

public class LocalPlayer : MonoBehaviour
{
    public float moveSpeed = 4.0f;

    private Rigidbody2D _rb;
    private IJsonRpcApiClient _fullNodeClient;
    private IJsonRpcApiClient _gatewayClient;
    private Vector2 _lastPosition = Vector2.zero;
    private ulong _sequenceNumber;
    
    async void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _fullNodeClient = new SuiJsonRpcApiClient(new UnityWebRequestRpcClient(SuiConstants.DEVNET_FULLNODE_ENDPOINT));
        _gatewayClient = new SuiJsonRpcApiClient(new UnityWebRequestRpcClient(SuiConstants.DEVNET_GATEWAY_ENDPOINT));
        _sequenceNumber = 0;
        
        var onChainStateObjectId = "";
        if (PlayerPrefs.HasKey(Constants.ONCHAIN_STATE_OBJECT_ID_KEY))
        {
            onChainStateObjectId = PlayerPrefs.GetString(Constants.ONCHAIN_STATE_OBJECT_ID_KEY);
        }
        if (!string.IsNullOrWhiteSpace(onChainStateObjectId))
        {
            await ExecuteMoveCallTxAsync(Constants.PACKAGE_OBJECT_ID, Constants.MOVEMENT_MODULE_NAME, "reset",
                new object[] { onChainStateObjectId});
        }
        
        StartCoroutine(UpdateOnChainPlayerStateWorker());
    }

    void Update()
    {
        _rb.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * moveSpeed;
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
            var task = UpdateOnChainPlayerStateAsync(_rb.position, _rb.velocity);
            yield return new WaitUntil(()=> task.IsCompleted);
        }
    }
     
    private async Task UpdateOnChainPlayerStateAsync(Vector2 position, Vector2 velocity)
    {
        if (_lastPosition == position && velocity.magnitude < 1.0f)
        {
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

        var onChainPosition = new OnChainVector2(position);
        var onChainVelocity = new OnChainVector2(velocity);
        var args = new object[] { onChainStateObjectId, onChainPosition.x, onChainPosition.y, onChainVelocity.x, onChainVelocity.y, _sequenceNumber++ };
        
        await ExecuteMoveCallTxAsync(Constants.PACKAGE_OBJECT_ID, Constants.MOVEMENT_MODULE_NAME, "do_update", args);
        _lastPosition = position;
    }

    private async Task ExecuteMoveCallTxAsync(string packageObjectId, string module, string function, object[] args)
    {
        var signer = SuiWallet.GetActiveAddress();

        if (string.IsNullOrWhiteSpace(signer))
        {
            Debug.Log("signer is null UpdateOnChainPlayerStateAsync early return");
            return;
        }

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
        
       // Debug.Log("ExecuteMoveCallTxAsync: " + JsonConvert.SerializeObject(args));
        
        var rpcResult = await _gatewayClient.MoveCallAsync(signer, packageObjectId, module, function, typeArgs, args, gasObjectId, 2000); 

        if (rpcResult.IsSuccess) 
        { 
            var txBytes = rpcResult.Result.TxBytes;
            await ExecuteTransactionAsync(txBytes);
        } 
        else 
        { 
            Debug.LogError("Something went wrong with the move call: " + rpcResult.ErrorMessage);
        }
    }
}
