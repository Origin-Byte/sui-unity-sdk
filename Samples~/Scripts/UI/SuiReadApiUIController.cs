using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json;
using Suinet.Rpc.Types;

public class SuiReadApiUIController : MonoBehaviour
{
    public Button GetObjectsOwnedByAddressButton;
    public TMP_InputField Input;
    public TMP_InputField Ouput; // outputs can be copy-pasted

    private void Start()
    {
        Input.text = SuiWallet.GetActiveAddress();

        GetObjectsOwnedByAddressButton.onClick.AddListener(async () =>
        {
            var address = Input.text;
            var filter = ObjectDataFilterFactory.CreateMatchAllFilter(ObjectDataFilterFactory.CreateAddressOwnerFilter(address));
            var ownedObjectsResult = await SuiApi.Client.GetOwnedObjectsAsync(address,
                new ObjectResponseQuery() { Filter = filter }, null, null);
            Ouput.text = JsonConvert.SerializeObject(ownedObjectsResult.Result, Formatting.Indented);
        });
    }
}
