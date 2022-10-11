using System;
using System.Collections;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Suinet.Rpc;
using Suinet.Rpc.Types;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class LocalPlayer : MonoBehaviour
{
    public float moveSpeed = 9.0f;

    private Rigidbody2D _rb;
    private IJsonRpcApiClient _fullNodeClient;
    private IJsonRpcApiClient _gatewayClient;
    private Vector2 _lastPosition = Vector2.zero;
    private ulong _sequenceNumber;
    private ExplosionController _explosionController;
    private bool _scoreboardUpdated = false;

    void Awake()
    {
        var randomPosition = GetRandomSpawnPosition();
        transform.position = randomPosition;
    }

    async void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _explosionController = GetComponent<ExplosionController>();
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
           
            var randomOnChainPosition = new OnChainVector2( transform.position);
            await ExecuteMoveCallTxAsync(Constants.PACKAGE_OBJECT_ID, Constants.MODULE_NAME, "reset",
                new object[] { onChainStateObjectId, randomOnChainPosition.x, randomOnChainPosition.y, TimestampService.UtcTimestamp}, false);
        }
        else 
        { 
            Debug.LogWarning("onChainStateObjectId is null, could not call reset.");
            return;
        }
        
        _rb.velocity = Vector2.up * moveSpeed;
        _scoreboardUpdated = false;
        StartCoroutine(UpdateOnChainPlayerStateWorker());
        //StartCoroutine(ExplodeAfterDelay(10));
    }

    private Vector2 GetRandomSpawnPosition()
    {
        const int MAX_POSITION_VALUE = 400;
        return new Vector2(Random.Range(-MAX_POSITION_VALUE, MAX_POSITION_VALUE),
            Random.Range(-MAX_POSITION_VALUE, MAX_POSITION_VALUE));
    }

    private IEnumerator ExplodeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _explosionController.Explode();
    }

    void Update()
    {
        var dir = 0f;
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            dir = 1f;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            dir = -1f;
        }

        if (dir != 0f)
        {
            var currentRot = transform.rotation.eulerAngles;
            currentRot.z += 90.0f * dir;
            transform.rotation = Quaternion.Euler(currentRot);
            _rb.velocity = _rb.velocity.Rotate(90.0f * dir);
        }
    }

    private async Task ExecuteTransactionAsync(string txBytes)
    {
        var keyPair = SuiWallet.GetActiveKeyPair(); 
 
        var signature = keyPair.Sign(txBytes); 
        var pkBase64 = keyPair.PublicKeyBase64; 
 
        var txRpcResult = await _fullNodeClient.ExecuteTransactionAsync(txBytes, SuiSignatureScheme.ED25519, signature, pkBase64); 
        if (!txRpcResult.IsSuccess)
        { 
            Debug.LogError("Something went wrong when executing the transaction: " + txRpcResult.ErrorMessage);
        }
        else
        {
//            Debug.Log(JsonConvert.SerializeObject(txRpcResult));
        }
    }
    
    private IEnumerator UpdateOnChainPlayerStateWorker() 
    { 
        while (true)
        {
            var position = _rb.position;
            var velocity = _rb.velocity;
            var task = UpdateOnChainPlayerStateAsync(position, velocity);
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

        //Debug.Log($"lp position: {position}, velocity: {velocity}");
        var onChainPosition = new OnChainVector2(position);
        var onChainVelocity = new OnChainVector2(velocity);
        var args = new object[] { onChainStateObjectId, onChainPosition.x, onChainPosition.y, onChainVelocity.x, onChainVelocity.y, _sequenceNumber++, _explosionController.IsExploded, TimestampService.UtcTimestamp };
//        Debug.Log($"lp onChainPosition.x: {onChainPosition.x}, onChainPosition.y: {onChainPosition.y}, onChainVelocity.x: {onChainVelocity.x}, onChainVelocity.y {onChainVelocity.y}. isExploded: {_explosionController.IsExploded}");

        await ExecuteMoveCallTxAsync(Constants.PACKAGE_OBJECT_ID, Constants.MODULE_NAME, "do_update", args, false);

        _lastPosition = position;

        if (_explosionController.IsExploded && ! _scoreboardUpdated)
        {
            args = new object[] { onChainStateObjectId, Constants.SCOREBOARD_OBJECT_ID };

            await ExecuteMoveCallTxAsync(Constants.PACKAGE_OBJECT_ID, Constants.MODULE_NAME, "add_to_scoreboard", args, false);

            _scoreboardUpdated = true;
        }
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
            gasObjectId = (await SuiHelper.GetCoinObjectIdsAboveBalancesOwnedByAddressAsync(_gatewayClient, signer, 1, 10000))[0]; 
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
