using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Suinet.Rpc;
using Suinet.Rpc.Types;
using UnityEngine;

public class LocalPlayerTile : MonoBehaviour
{
    public float moveSpeed = 8.0f;
    private const string PACKAGE_OBJECT_ID = "0xddd7d5858fabb04ea48da290d9f9031b7bd645e3";
    private const string ONCHAIN_STATE_OBJECT_ID_KEY = PACKAGE_OBJECT_ID + "_onChainStateObject";
    private string _onChainStateObjectId = "";

    private Rigidbody2D rb;
    private IJsonRpcApiClient _fullNodeClient;
    private IJsonRpcApiClient _gatewayClient;
    private const int UpdatePeriodInMs = 500;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _fullNodeClient = new SuiJsonRpcApiClient(new UnityWebRequestRpcClient(SuiConstants.DEVNET_FULLNODE_ENDPOINT));
        _gatewayClient = new SuiJsonRpcApiClient(new UnityWebRequestRpcClient(SuiConstants.DEVNET_GATEWAY_ENDPOINT));
        
        if (PlayerPrefs.HasKey(ONCHAIN_STATE_OBJECT_ID_KEY)) 
        { 
            _onChainStateObjectId = PlayerPrefs.GetString(ONCHAIN_STATE_OBJECT_ID_KEY); 
        } 
        
        StartCoroutine(UpdateOnChainPlayerStateWorker());
    }

    void Update()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * moveSpeed;
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
            var waitTask = Task.Delay(UpdatePeriodInMs);
            yield return new WaitUntil(()=> task.IsCompleted && waitTask.IsCompleted);
        }
    }
    
    private async Task UpdateOnChainPlayerStateAsync(Vector2 position, Vector2 velocity)
    {
        //if (_ongoingRequestIds.Count > 0) return;
        
        //Debug.Log("DoMoveOnChainAsync");
        
        // var requestId = Guid.NewGuid().ToString();
        // _ongoingRequestIds.Add(requestId);
        
        var signer = SuiWallet.GetActiveAddress(); 
        var packageObjectId = PACKAGE_OBJECT_ID; 
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
 
        var args = new object[] { _onChainStateObjectId, Convert.ToUInt64((position.x + 100000) * 1000), 
            Convert.ToUInt64((position.y + 100000) * 1000), Convert.ToUInt64((velocity.x + 100000) * 1000), Convert.ToUInt64((velocity.y + 100000) * 1000) };
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
}
