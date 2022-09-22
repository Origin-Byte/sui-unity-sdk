using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WalletUIController : MonoBehaviour
{
    public Button NewWalletButton;
    public TMP_InputField NewWalletMnemonicsText;

    public Button ImportWalletButton;
    public TMP_InputField MnemonicsInputField;

    public TMP_InputField ActiveAddressText;


    private void Start()
    {
        ActiveAddressText.text = SuiWallet.GetActiveAddress();

        NewWalletButton.onClick.AddListener(() =>
        {
            var walletmnemo = SuiWallet.CreateNewWallet();
            NewWalletMnemonicsText.gameObject.SetActive(true);
            NewWalletMnemonicsText.text = walletmnemo;

            ActiveAddressText.text = SuiWallet.GetActiveAddress();
        });


        ImportWalletButton.onClick.AddListener(() =>
        {
            NewWalletMnemonicsText.gameObject.SetActive(false);
            SuiWallet.RestoreWalletFromMnemonics(MnemonicsInputField.text);
            ActiveAddressText.text = SuiWallet.GetActiveAddress();
        });
    }
}
