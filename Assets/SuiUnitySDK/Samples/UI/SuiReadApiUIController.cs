using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Suinet.Rpc;
using Suinet.Rpc.Types;
using Newtonsoft.Json;

public class SuiReadApiUIController : MonoBehaviour
{
    public Button GetObjectsOwnedByAddressButton;
    public TMP_InputField Input;
    public TMP_InputField Ouput; // outputs can be copy-pasted

    private void Start()
    {
        Input.text = SuiWallet.Instance.GetActiveAddress();

        GetObjectsOwnedByAddressButton.onClick.AddListener(async () =>
        {
            var rpcClient = new UnityWebRequestRpcClient(SuiConstants.DEVNET_ENDPOINT);
            var suiJsonRpcApi = new SuiJsonRpcApiClient(rpcClient);

            var ownedObjectsResult = await suiJsonRpcApi.GetObjectsOwnedByAddressAsync(Input.text);

            Ouput.text = JsonConvert.SerializeObject(ownedObjectsResult.Result, Formatting.Indented);
        });
    }
}
