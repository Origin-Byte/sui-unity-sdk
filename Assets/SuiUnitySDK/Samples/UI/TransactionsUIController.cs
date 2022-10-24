using Newtonsoft.Json;
using Suinet.Rpc;
using Suinet.Rpc.Types;
using System.Threading.Tasks;
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

    private string SharedCounterObjectId = "0x97b46235b024f9ec345f4669ac5a519bb574a84a";
    private string PackageObjectId = "0x357e72c66f0755e08a1dbc8d80c1460d9da78a99";

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
                Gas = (await SuiHelper.GetCoinObjectIdsAboveBalancesOwnedByAddressAsync(SuiApi.Client, signer, 1, 10000))[0],
                GasBudget = 5000,
                RequestType = SuiExecuteTransactionRequestType.WaitForEffectsCert
            };
           
            await SuiApi.Signer.SignAndExecuteMoveCallAsync(moveCallTx);
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
