using Newtonsoft.Json;
using Suinet.Rpc;
using Suinet.Rpc.Types;
using System.Threading.Tasks;
using Suinet.Wallet;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Move contract used by this sample:
/// https://github.com/MystenLabs/sui/blob/main/sui_programmability/examples/basics/sources/counter.move
/// </summary>
public class TransactionsUIController : MonoBehaviour
{
    public Button IncrementCounterButton;
    public Button RefreshCounterButton;

    public TMP_InputField Output;

    private string SharedCounterObjectId = "0xab86eab42d95c987c879fb53292fa47210e30190524e07e4cf7aa9930446b538";
    private string PackageObjectId = "0x116c6862df1e71aa13a88e34b460cfdd46d3fc21bbe64df546faea7251b25dce";

    private async void Start()
    {
        IncrementCounterButton.onClick.AddListener(async () =>
        {
            var signer = SuiWallet.GetActiveAddress();
            var moveCallTx = new MoveCallTransaction()
            {
                Signer = signer,
                PackageObjectId = PackageObjectId,
                Module = "counter",
                Function = "increment",
                TypeArguments = ArgumentBuilder.BuildTypeArguments(),
                Arguments = ArgumentBuilder.BuildArguments( SharedCounterObjectId ),
                Gas =null,
                GasBudget = 10000000,
                RequestType = ExecuteTransactionRequestType.WaitForLocalExecution
            };
           
            var moveCallResult = await SuiApi.Client.MoveCallAsync(moveCallTx);

            var txBytes = moveCallResult.Result.TxBytes;
            var rawSigner = new RawSigner(SuiWallet.GetActiveKeyPair());
            var signature = rawSigner.SignData(Intent.GetMessageWithIntent(txBytes));
          
            var txResponse = await SuiApi.Client.ExecuteTransactionBlockAsync(txBytes, new[] { signature.Value }, TransactionBlockResponseOptions.ShowAll(), ExecuteTransactionRequestType.WaitForLocalExecution); 
            
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
        var rpcClient = new UnityWebRequestRpcClient(SuiConstants.TESTNET_FULLNODE);
        var suiJsonRpcApi = new SuiJsonRpcApiClient(rpcClient);

        var rpcResult = await suiJsonRpcApi.GetObjectAsync(SharedCounterObjectId, ObjectDataOptions.ShowAll());
        Output.text = JsonConvert.SerializeObject(rpcResult.Result, formatting: Formatting.Indented);
    }
}
