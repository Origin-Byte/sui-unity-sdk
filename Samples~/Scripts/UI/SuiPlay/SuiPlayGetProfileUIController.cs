using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json;
using Suinet.SuiPlay.Requests;

public class SuiPlayGetProfileUIController : MonoBehaviour
{
    public Button actionButton;
    public TMP_InputField ouput; // outputs can be copy-pasted

    private void Start()
    {
        actionButton.onClick.AddListener(async () =>
        {
            var result = await SuiPlay.Client.GetPlayerProfileAsync(SuiPlayConfig.GAME_ID);
            ouput.text = JsonConvert.SerializeObject(result, Formatting.Indented);
        });
    }
}
