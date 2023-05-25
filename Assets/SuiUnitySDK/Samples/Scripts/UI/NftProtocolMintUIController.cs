using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Suinet.NftProtocol;
using Suinet.NftProtocol.Examples;
using Suinet.NftProtocol.TransactionBuilders;
using Suinet.Rpc.Signer;
using Suinet.Rpc.Types;
using Suinet.Rpc.Types.MoveTypes;
using Suinet.Wallet;
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
    public TMP_Text NFTMintedText;
    public TMP_InputField NFTPackageObjectIdField;
    public TMP_InputField NFTMintedReadonlyInputField;
    public TMP_InputField TargetWalletAddressInputField;

    public Image NFTImage;
    
    private void Start()
    {
        NFTPackageObjectIdField.text = "0xb11eda772add7178d97d98fbcb5dc73ea1afec0bb94705416c43efdbedba6e4b";
        TargetWalletAddressInputField.text = SuiWallet.GetActiveAddress();
        
        MintNFTButton.onClick.AddListener(async () =>
        {
            NFTMintedText.gameObject.SetActive(false);
            NFTMintedReadonlyInputField.gameObject.SetActive(false);

            // anyone can mint from this contract
            var keypair = SuiWallet.GetActiveKeyPair();
            var nftProtocolClient = new NftProtocolClient(SuiApi.Client, SuiWallet.GetActiveKeyPair());

            var randomFaceIndex = Random.Range(1, 9);
            var txParams = new MintSuitradersNft()
            {
                Attributes = new Dictionary<string, object>()
                {
                    { "nft_type", "face" },
                },
                Description = "You can use this as a face of your character in the game!",
                Recipient = TargetWalletAddressInputField.text,
                ModuleName =  "suitraders",
                Function = "airdrop_nft",
                Name = $"Face {randomFaceIndex}",
                PackageObjectId = NFTPackageObjectIdField.text,
                Signer = keypair.PublicKeyAsSuiAddress,
                Url = $"https://suiunitysdksample.blob.core.windows.net/nfts/face{randomFaceIndex}.png"
            };

            // if we pass null, it will automatically select a gas object
            var mintRpcResult = await nftProtocolClient.MintNftAsync(txParams, null);
            if (mintRpcResult is { IsSuccess: true })
            {
                // let's get the art nft, may have to skip the new coin created 
                Debug.Log("mintRpcResult status: " + mintRpcResult.Result.Effects.Status.Status);

                Debug.Log("number of objects created: " + mintRpcResult.Result.Effects.Created.Count);
                foreach (var createdRef in mintRpcResult.Result.Effects.Created)
                {
                    var nftId = createdRef.Reference.ObjectId;
                    if (!string.IsNullOrWhiteSpace(nftId))
                    {
                        await Task.Delay(3000); // wait a bit... leave time to the blockchain to process the transaction
                        var artNftResult = await SuiApi.NftProtocolClient.GetArtNftAsync(nftId);
                        if (artNftResult.IsSuccess && artNftResult.Result != null)
                        {
                            Debug.Log("this might be good: " + nftId);

                            await LoadNFT(artNftResult.Result.Url);
                            NFTMintedText.gameObject.SetActive(true);
                            NFTMintedReadonlyInputField.gameObject.SetActive(true);

                            NFTMintedReadonlyInputField.text = "https://suiexplorer.com/object/" + nftId + "?network=testnet";
                            return;
                        }
                        else
                        {
                            Debug.Log("skipping, this is not artnft: " + artNftResult.ErrorMessage);
                        }
                    }
                    else
                    {
                        Debug.LogError("Something went wrong with the minting 2: " + mintRpcResult.RawRpcResponse);
                    }
                }
            }
            else
            {
                Debug.LogError("Something went wrong with the minting 1: " + mintRpcResult.ErrorMessage);
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
