using Newtonsoft.Json.Linq;
using Suinet.Rpc.Types;
using System.Threading.Tasks;
using Suinet.Rpc;
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
    public TMP_InputField NFTCollectionIdInputField;
    public TMP_InputField NFTMintedReadonlyInputField;

    public Image NFTImage;


    private void Start()
    {
        // Default text
        NFTNameInputField.text = "Origin Byte NFT using Nft protocol";
        NFTDescriptionInputField.text = "NFT minted using SuiUnitySDK by Origin Byte";
        NFTUrlInputField.text = "https://avatars.githubusercontent.com/u/112119979";
        // Origin Byte NFTs are minted for a given collection. Default collection for samples.
        NFTCollectionIdInputField.text = "0x0f31c2ef91697057ad51c7914bc90be5b43569a5";

        MintNFTButton.onClick.AddListener(async () =>
        {
            var signer = SuiWallet.GetActiveAddress();
            // package id of the Nft Protocol
            var packageObjectId = "0x4112429a20b9f6bfcc5f43e773dbbf71053cdfa5";
            var module = "std_nft";
            var function = "mint_and_transfer";
            var typeArgs = System.Array.Empty<string>();

            // We need 2 separate gas objects because both of them will be mutated in a batch transaction
            var gasObjectIds = await SuiHelper.GetCoinObjectIdsAboveBalancesOwnedByAddressAsync(SuiApi.GatewayClient, signer, 2);

            if (gasObjectIds.Count < 2)
            {
                Debug.LogError("Could not retrieve 2 sui coin objects with at least 1000 balance. Please send more SUI to your address");
                return;
            }

            var args = new object[] { NFTNameInputField.text, NFTUrlInputField.text, false, new object[] { "description" },
                new object[] { NFTDescriptionInputField.text }, NFTCollectionIdInputField.text, gasObjectIds[0], signer };

            NFTMintedText.gameObject.SetActive(false);
            NFTMintedReadonlyInputField.gameObject.SetActive(false);
            var rpcResult = await SuiApi.GatewayClient.MoveCallAsync(signer, packageObjectId, module, function, typeArgs, args, gasObjectIds[1], 2000);

            if (rpcResult.IsSuccess)
            {
                var keyPair = SuiWallet.GetActiveKeyPair();

                var txBytes = rpcResult.Result.TxBytes;
                var signature = keyPair.Sign(rpcResult.Result.TxBytes);
                var pkBase64 = keyPair.PublicKeyBase64;

                var txRpcResult = await SuiApi.GatewayClient.ExecuteTransactionAsync(txBytes, SuiSignatureScheme.ED25519, signature, pkBase64);
                if (txRpcResult.IsSuccess)
                {
                    await LoadNFT(NFTUrlInputField.text);
                    NFTMintedText.gameObject.SetActive(true);
                    NFTMintedReadonlyInputField.gameObject.SetActive(true);

                    var txEffects = JObject.FromObject(txRpcResult.Result.Effects);

                    // check the created object, one is a coin, the other one is the actual NFT
                    var mintedNftObjectId = txEffects.SelectToken("created[0].reference.objectId").Value<string>();
                    var createdObject = await SuiApi.GatewayClient.GetObjectAsync(mintedNftObjectId);
                    var createdJobj = JObject.FromObject(createdObject.Result.Details);
                    if (createdJobj.SelectToken("data.type").Value<string>() == "0x2::coin::Coin<0x2::sui::SUI>")
                    {
                        mintedNftObjectId = txEffects.SelectToken("created[1].reference.objectId").Value<string>();
                    }

                    NFTMintedReadonlyInputField.text = "https://explorer.devnet.sui.io/objects/"+ mintedNftObjectId;
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
