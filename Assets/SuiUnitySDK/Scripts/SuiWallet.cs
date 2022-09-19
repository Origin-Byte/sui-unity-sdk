using Suinet.Wallet;
using UnityEngine;

/// <summary>
/// Uses PlayerPrefs as store
/// </summary>
public class SuiWallet : MonoBehaviour
{
    private const string MnemonicsKey = "MnemonicsKey";
    public static SuiWallet Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public string GetActiveAddress()
    {
        var keypair = GetActiveKeyPair();
        if (keypair != null)
        {
            return keypair.PublicKeySuiAddress;
        }

        return "0x";
    }

    public string CreateNewWallet()
    {
        var mnemo = Mnemonics.GenerateMnemonic();
        RestoreWalletFromMnemonics(mnemo);
        return mnemo;
    }

    public bool RestoreWalletFromMnemonics(string mnemo)
    {
        if (!Mnemonics.ValidateMnemonic(mnemo)) return false;

        PlayerPrefs.SetString(MnemonicsKey, mnemo);
        PlayerPrefs.Save();
        return true;
    }

    public Ed25519KeyPair GetActiveKeyPair()
    {
        if (PlayerPrefs.HasKey(MnemonicsKey))
        {
            var mnemo = PlayerPrefs.GetString(MnemonicsKey);
            return Mnemonics.GetKeypairFromMnemonic(mnemo);
        }

        return null;
    }

    public void Logout()
    {
        PlayerPrefs.DeleteKey(MnemonicsKey);
        PlayerPrefs.Save();
    }
}
