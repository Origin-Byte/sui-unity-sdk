using System.Linq;
using Newtonsoft.Json.Linq;
using Suinet.Rpc.Types;
using System.Threading.Tasks;
using Suinet.NftProtocol;
using Suinet.NftProtocol.Launchpad;
using Suinet.NftProtocol.Launchpad.Market;
using Suinet.NftProtocol.Nft;
using Suinet.NftProtocol.TransactionBuilders;
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
    public TMP_InputField NFTLaunchpadIdInputField;
    public TMP_InputField NFTMintedReadonlyInputField;

    public Image NFTImage;


    private void Start()
    {
        // Default text
        //NFTNameInputField.text = "Origin Byte NFT using Nft protocol";
        //NFTDescriptionInputField.text = "NFT minted using SuiUnitySDK by Origin Byte";
        //NFTUrlInputField.text = "https://avatars.githubusercontent.com/u/112119979";
        // Origin Byte NFTs are claimed from a launchpad, that is created for a collection.
        NFTLaunchpadIdInputField.text = "0x9f1f5e2a86b8c9a6904ee30b585f82f387466120";

        MintNFTButton.onClick.AddListener(async () =>
        {
            NFTMintedText.gameObject.SetActive(false);
            NFTMintedReadonlyInputField.gameObject.SetActive(false);
            
            var signer = SuiWallet.GetActiveAddress();
            var launchpadId = NFTLaunchpadIdInputField.text;
            var packageObjectId = "0x383ddb4a14b8ea9be1e0c2bb279da4aed47b2033";
            var moduleName = "suimarines";
            var collectionType = $"{packageObjectId}::suimarines::SUIMARINES";
            
            var launchpadResult = await SuiApi.Client.GetObjectAsync<FixedPriceMarket>(launchpadId);

            // var buyNftTxBuilder = new BuyNftCertificate()
            // {
            //     Signer = signer,
            //     Wallet = (await SuiHelper.GetCoinObjectIdsAboveBalancesOwnedByAddressAsync(SuiApi.Client, signer, 1))[0],
            //     LaunchpadId = launchpadId,
            //     PackageObjectId = packageObjectId,
            //     CollectionType = collectionType,
            //     ModuleName = moduleName
            // };
            //
            // var buyCertResponse = await SuiApi.NftProtocolClient.BuyNftCertificateAsync(buyNftTxBuilder);
            // var certificateId = buyCertResponse.Result.EffectsCert.Effects.Effects.Created.First().Reference.ObjectId;
            // var buyCertificateRpcResult = await SuiApi.Client.GetObjectAsync<NftCertificate>(certificateId);
            // var nftId = buyCertificateRpcResult.Result.NftId;
            //
            // var claimNftTxBuilder = new ClaimNftCertificate()
            // {
            //     Signer = signer,
            //     LaunchpadId = launchpadId,
            //     PackageObjectId = packageObjectId,
            //     CollectionType = collectionType,
            //     ModuleName = moduleName,
            //     Recipient = signer,
            //     CertificateId = buyCertResponse.Result.EffectsCert.Effects.Effects.Created.First().Reference.ObjectId,
            //     NftId = nftId,
            //     NftType = "unique_nft::Unique"
            // };
            //
            // var claimCertResult = await SuiApi.NftProtocolClient.CaimNftCertificateAsync(claimNftTxBuilder);
            //
            // if (claimCertResult.IsSuccess)
            // {
            //     var nftResult = await SuiApi.Client.GetObjectAsync<UniqueNft>(nftId);
            //     await LoadNFT(nftResult.Result.Data.Fields.Url);
            //     NFTMintedText.gameObject.SetActive(true);
            //     NFTMintedReadonlyInputField.gameObject.SetActive(true);
            //
            //     NFTMintedReadonlyInputField.text = "https://explorer.devnet.sui.io/objects/"+ nftId;
            // }
            // else
            // {
            //     Debug.LogError("Something went wrong with the claiming: " + claimCertResult.ErrorMessage);
            // }
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
