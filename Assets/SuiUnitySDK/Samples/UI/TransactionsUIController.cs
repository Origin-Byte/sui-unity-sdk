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

    private string SharedCounterObjectId = "0xe5de6824f7c0cedc2489cc06d7e7f2e426edbe42";

    private async void Start()
    {

        IncrementCounterButton.onClick.AddListener(async () =>
        {
            var signer = SuiWallet.GetActiveAddress();
            var packageObjectId = "0x6295af8a599ee4d9e7addc650d8e2a25c9046a37";
            var module = "counter";
            var function = "increment";
            var typeArgs = System.Array.Empty<string>();
            var args = new object[] { SharedCounterObjectId };
            var gasObjectId = GasObjectIdInput.text;
            var rpcResult = await SuiApi.GatewayClient.MoveCallAsync(signer, packageObjectId, module, function, typeArgs, args, gasObjectId, 2000);
            var keyPair = SuiWallet.GetActiveKeyPair();

            var txBytes = rpcResult.Result.TxBytes;
            var signature = keyPair.Sign(rpcResult.Result.TxBytes);
            var pkBase64 = keyPair.PublicKeyBase64;

            await SuiApi.GatewayClient.ExecuteTransactionAsync(txBytes, SuiSignatureScheme.ED25519, signature, pkBase64);
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
        var rpcClient = new UnityWebRequestRpcClient(SuiConstants.DEVNET_GATEWAY_ENDPOINT);
        var suiJsonRpcApi = new SuiJsonRpcApiClient(rpcClient);

        var rpcResult = await suiJsonRpcApi.GetObjectAsync(SharedCounterObjectId);
        Output.text = JsonConvert.SerializeObject(rpcResult.Result, formatting: Formatting.Indented);
    }
}
