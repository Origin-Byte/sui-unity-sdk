using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json;

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
            var ownedObjectsResult = await SuiApi.Client.GetObjectsOwnedByAddressAsync(address);
            Ouput.text = JsonConvert.SerializeObject(ownedObjectsResult.Result, Formatting.Indented);
        });
    }
}
