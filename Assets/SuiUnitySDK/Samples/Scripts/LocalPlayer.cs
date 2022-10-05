using System;
using System.Collections;
using System.Threading.Tasks;
using Suinet.Rpc;
using Suinet.Rpc.Types;
using UnityEngine;

public static class Vector2Extension
{
    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        return Quaternion.Euler(0, 0, degrees) * v;
    }
}

public class LocalPlayer : MonoBehaviour
{
    public float moveSpeed = 4.0f;

    private Rigidbody _rb;
    private IJsonRpcApiClient _fullNodeClient;
    private IJsonRpcApiClient _gatewayClient;
    private Vector2 _lastPosition = Vector2.zero;
    private ulong _sequenceNumber;
    
    async void Start()
    {
        _rb = GetComponent<Rigidbody>();
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
                new object[] { onChainStateObjectId}, false);
        }
        
        StartCoroutine(UpdateOnChainPlayerStateWorker());
        _rb.velocity = Vector3.forward * moveSpeed;
    }

    void Update()
    {
        var dir = 0f;
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            dir = -1f;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            dir = 1f;
        }

        if (dir != 0f)
        {
            var currentRot = transform.rotation.eulerAngles;
            currentRot.y += 90.0f * dir;
            transform.rotation = Quaternion.Euler(currentRot);
            _rb.velocity = Quaternion.Euler(0, 90.0f * dir, 0) * _rb.velocity;
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
            var position = _rb.position;
            var posVec2 = new Vector3(position.x, position.z);
            var velocity = _rb.velocity;
            var velVec2 = new Vector3(velocity.x, velocity.z);
            var task = UpdateOnChainPlayerStateAsync(posVec2, velVec2);
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
        
        await ExecuteMoveCallTxAsync(Constants.PACKAGE_OBJECT_ID, Constants.MOVEMENT_MODULE_NAME, "do_update", args, true);
        _lastPosition = position;
    }

    private async Task ExecuteMoveCallTxAsync(string packageObjectId, string module, string function, object[] args, bool immediateReturn)
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
        
        var rpcResult = await _gatewayClient.MoveCallAsync(signer, packageObjectId, module, function, typeArgs, args, gasObjectId, 2000); 

        if (rpcResult.IsSuccess) 
        { 
            var txBytes = rpcResult.Result.TxBytes;
            if (immediateReturn)
            {
                ExecuteTransactionAsync(txBytes); // intentionally not awaiting
            }
            else
            {
                await ExecuteTransactionAsync(txBytes);
            }
        } 
        else 
        { 
            Debug.LogError("Something went wrong with the move call: " + rpcResult.ErrorMessage);
        }
    }
}
