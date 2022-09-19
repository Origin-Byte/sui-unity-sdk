using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WalletUI : MonoBehaviour
{
    public Button NewWalletButton;
    public TMP_InputField NewWalletMnemonicsText;

    public Button ImportWalletButton;
    public TMP_InputField MnemonicsInputField;

    public TMP_InputField ActiveAddressText;


    private void Start()
    {
        ActiveAddressText.text = SuiWallet.Instance.GetActiveAddress();

        NewWalletButton.onClick.AddListener(() =>
        {
            var walletmnemo = SuiWallet.Instance.CreateNewWallet();
            NewWalletMnemonicsText.gameObject.SetActive(true);
            NewWalletMnemonicsText.text = walletmnemo;

            ActiveAddressText.text = SuiWallet.Instance.GetActiveAddress();
        });


        ImportWalletButton.onClick.AddListener(() =>
        {
            NewWalletMnemonicsText.gameObject.SetActive(false);
            SuiWallet.Instance.RestoreWalletFromMnemonics(MnemonicsInputField.text);
            ActiveAddressText.text = SuiWallet.Instance.GetActiveAddress();
        });
    }
}
