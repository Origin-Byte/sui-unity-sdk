using Newtonsoft.Json;
using Suinet.Rpc;
using Suinet.Rpc.Types;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// Mint an NFT using Origin Byte NFT Protocol
/// </summary>
public class NftProtocolMintUIController : MonoBehaviour
{
    public Button MintNFTButton;
    public TMP_InputField NFTNameInputField;
    public TMP_InputField NFTDescriptionInputField;
    public TMP_InputField NFTUrlInputField;
    public TMP_Text NFTMintedText;
    public TMP_InputField NFTCollectionReadonlyInputField;
    public TMP_InputField NFTMintedReadonlyInputField;


    public Image NFTImage;

    // Origin Byte NFTs are minted for a collection
    private const string _collectionObjectId = "0xd1211d906949786592de54daae392a49edfe197e";


    private async void Start()
    {
        // Default text
        NFTNameInputField.text = "Origin Byte NFT using Nft protocol";
        NFTDescriptionInputField.text = "NFT minted using SuiUnitySDK by Origin Byte";
        NFTUrlInputField.text = "https://avatars.githubusercontent.com/u/112119979";
        NFTCollectionReadonlyInputField.text = "https://explorer.devnet.sui.io/objects/0xd1211d906949786592de54daae392a49edfe197e";

        MintNFTButton.onClick.AddListener(async () =>
        {
            var rpcClient = new UnityWebRequestRpcClient(SuiConstants.DEVNET_ENDPOINT);
            var suiJsonRpcApi = new SuiJsonRpcApiClient(rpcClient);

            var signer = SuiWallet.Instance.GetActiveAddress();
            // package id of the Nft Protocol
            var packageObjectId = "0x1e5a734576e8d8c885cd4cf75665c05d9944ae34";
            var module = "std_nft";
            var function = "mint_and_transfer";
            var typeArgs = System.Array.Empty<string>();

            // We need 2 separate gas objects because both of them will be mutated in a batch transaction
            var gas1Id = "0x8c588303884be101aedf6cfda8696347bcfb11c4";
            var gas2Id = "0xff14d4f36c0fc91473a60188a5747c9d0d51d67a";

            var args = new object[] { NFTNameInputField.text, NFTUrlInputField.text, false, new object[] { "description" },
                new object[] { NFTDescriptionInputField.text }, _collectionObjectId, gas1Id, signer };

            NFTMintedText.gameObject.SetActive(false);
            NFTMintedReadonlyInputField.gameObject.SetActive(false);
            var rpcResult = await suiJsonRpcApi.MoveCallAsync(signer, packageObjectId, module, function, typeArgs, args, gas2Id, 2000);

            if (rpcResult.IsSuccess)
            {
                var keyPair = SuiWallet.Instance.GetActiveKeyPair();

                var txBytes = rpcResult.Result.TxBytes;
                var signature = keyPair.Sign(rpcResult.Result.TxBytes);
                var pkBase64 = keyPair.PublicKeyBase64;

                var txRpcResult = await suiJsonRpcApi.ExecuteTransactionAsync(txBytes, SuiSignatureScheme.ED25519, signature, pkBase64);
                if (txRpcResult.IsSuccess)
                {
                    await LoadNFT(NFTUrlInputField.text);
                    NFTMintedText.gameObject.SetActive(true);
                    NFTMintedReadonlyInputField.gameObject.SetActive(true);
                  //  NFTMintedReadonlyInputField.text = ;
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

    // TODO move this to its own service
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
