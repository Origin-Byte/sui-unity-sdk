using Suinet.Rpc.Types;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MintNFTUIController : MonoBehaviour
{
    public Button MintNFTButton;
    public TMP_InputField NFTNameInputField;
    public TMP_InputField NFTDescriptionInputField;
    public TMP_InputField NFTUrlInputField;
    public TMP_InputField GasObjectIdInputField;
    public TMP_Text NFTMintedText;

    public Image NFTImage;

    private void Start()
    {
        // Default text
        NFTNameInputField.text = "Origin Byte NFT";
        NFTDescriptionInputField.text = "NFT minted using SuiUnitySDK by Origin Byte";
        NFTUrlInputField.text = "https://avatars.githubusercontent.com/u/112119979";

        MintNFTButton.onClick.AddListener(async () =>
        {
            var signer = SuiWallet.GetActiveAddress();
            var packageObjectId = "0x2";
            var module = "devnet_nft";
            var function = "mint";
            var typeArgs = System.Array.Empty<string>();
            var args = new object[] { NFTNameInputField.text, NFTDescriptionInputField.text, NFTUrlInputField.text };
            var gasObjectId = GasObjectIdInputField.text;

            NFTMintedText.gameObject.SetActive(false);
            var rpcResult = await SuiApi.Client.MoveCallAsync(signer, packageObjectId, module, function, typeArgs, args, gasObjectId, 2000);

            if (rpcResult.IsSuccess)
            {
                var keyPair = SuiWallet.GetActiveKeyPair();

                var txBytes = rpcResult.Result.TxBytes;
                var signature = keyPair.Sign(rpcResult.Result.TxBytes);
                var pkBase64 = keyPair.PublicKeyBase64;

                var txRpcResult = await SuiApi.Client.ExecuteTransactionAsync(txBytes, SuiSignatureScheme.ED25519, signature, pkBase64, SuiExecuteTransactionRequestType.WaitForTxCert);
                if (txRpcResult.IsSuccess)
                {
                    await LoadNFT(NFTUrlInputField.text);
                    NFTMintedText.gameObject.SetActive(true);
                }
                else
                {
                    Debug.LogError("Something went wrong when executing the transaction: " + txRpcResult.ErrorMessage);
                }
            }
            else
            {
                Debug.LogError("Something went wrong with the move call: " + rpcResult.ErrorMessage);
            }
        });
    }

    private async Task LoadNFT(string url)
    {
        using var req = new UnityWebRequest(url, "GET");

        req.downloadHandler = new DownloadHandlerBuffer();
        req.SendWebRequest();
        while (!req.isDone)
        {
            await Task.Yield();
        }
        var data = req.downloadHandler.data;

        var tex = new Texture2D(256, 256);
        tex.LoadImage(data);
        var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
        NFTImage.sprite = sprite;
    }
}
