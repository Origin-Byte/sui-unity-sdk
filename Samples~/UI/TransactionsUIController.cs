using Newtonsoft.Json;
using Suinet.Rpc;
using Suinet.Rpc.Types;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransactionsUIController : MonoBehaviour
{
    public Button IncrementCounterButton;
    public Button RefreshCounterButton;
    public TMP_InputField GasObjectIdInput;

    public TMP_InputField Output;

    private string SharedCounterObjectId = "0xb583a88cea231f54348d0c4df4415fd04201529d";

    private async void Start()
    {

        IncrementCounterButton.onClick.AddListener(async () =>
        {
            var rpcClient = new UnityWebRequestRpcClient(SuiConstants.DEVNET_ENDPOINT);
            var suiJsonRpcApi = new SuiJsonRpcApiClient(rpcClient);

            var signer = SuiWallet.Instance.GetActiveAddress();
            var packageObjectId = "0xa21da7987c2b75870ddb4d638600f9af950b64c6";
            var module = "counter";
            var function = "increment";
            var typeArgs = System.Array.Empty<string>();
            var args = new object[] { SharedCounterObjectId };
            var gasObjectId = GasObjectIdInput.text;
            var rpcResult = await suiJsonRpcApi.MoveCallAsync(signer, packageObjectId, module, function, typeArgs, args, gasObjectId, 2000);
            var keyPair = SuiWallet.Instance.GetActiveKeyPair();

            var txBytes = rpcResult.Result.TxBytes;
            var signature = keyPair.Sign(rpcResult.Result.TxBytes);
            var pkBase64 = keyPair.PublicKeyBase64;

            await suiJsonRpcApi.ExecuteTransactionAsync(txBytes, SuiSignatureScheme.ED25519, signature, pkBase64);
            await RefreshCounter();
        });


        RefreshCounterButton.onClick.AddListener(async () =>
        {
            await RefreshCounter();
        });

        await RefreshCounter();
    }

    private async Task RefreshCounter()
    {
        var rpcClient = new UnityWebRequestRpcClient(SuiConstants.DEVNET_ENDPOINT);
        var suiJsonRpcApi = new SuiJsonRpcApiClient(rpcClient);

        var rpcResult = await suiJsonRpcApi.GetObjectAsync(SharedCounterObjectId);
        Output.text = JsonConvert.SerializeObject(rpcResult.Result, formatting: Formatting.Indented);
    }
}
