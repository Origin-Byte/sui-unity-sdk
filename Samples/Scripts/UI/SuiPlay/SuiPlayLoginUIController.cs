using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json;
using Suinet.SuiPlay.Requests;

public class SuiPlayLoginUIController : MonoBehaviour
{
    public Button actionButton;
    public TMP_InputField email;
    public TMP_InputField password;
    public TMP_InputField ouput; // outputs can be copy-pasted

    private void Start()
    {
        actionButton.onClick.AddListener(async () =>
        {
            var registrationRequest = new LoginRequest()
            {
                Email = email.text,
                Password = password.text,
                GameId = SuiPlayConfig.GAME_ID,
                StudioId = SuiPlayConfig.STUDIO_ID
            };
            
            var result = await SuiPlay.Client.LoginWithEmailAsync(registrationRequest);
            ouput.text = JsonConvert.SerializeObject(result, Formatting.Indented);
        });
    }
}
