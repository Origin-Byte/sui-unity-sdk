using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Suinet.NftProtocol;
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
    public TMP_InputField NFTMintCapIdField;
    public TMP_InputField NFTMintedReadonlyInputField;
    public TMP_InputField TargetWalletAddressInputField;

    public Image NFTImage;
    
    private const string SAMPLE_SIGNER_MNEMONIC = "bus indicate leave science minor clip embrace faculty wink industry addict track soup burger scissors another enrich muscle loop fever vacuum buyer paddle roof";

    private void Start()
    {
        NFTMintCapIdField.text = "0xa6feff4e40c90bf34abafed94d523744050df63b";
        TargetWalletAddressInputField.text = SuiWallet.GetActiveAddress();
        
        MintNFTButton.onClick.AddListener(async () =>
        {
            NFTMintedText.gameObject.SetActive(false);
            NFTMintedReadonlyInputField.gameObject.SetActive(false);
            
            var mintCapId = NFTMintCapIdField.text;
            
            var mintCapResult = await SuiApi.Client.GetObjectAsync(mintCapId);

            if (mintCapResult.IsSuccess)
            {
                var mintCapType = mintCapResult.Result.Object.Data.Type;

                var deadBytesTypeString = mintCapType.Struct.Replace("MintCap<", "");
                deadBytesTypeString = deadBytesTypeString.Remove(deadBytesTypeString.Length - 1, 1);
                var deadBytesType = new MoveType(deadBytesTypeString);
                
                // we can sign the mint transaction with the wallet that deployed the contract
                var signerKeyPair = Mnemonics.GetKeypairFromMnemonic(SAMPLE_SIGNER_MNEMONIC);
                var signer = new Signer(SuiApi.Client, signerKeyPair);
                var nftProtocolClient = new NftProtocolClient(SuiApi.Client, signer);

                var randomFaceIndex = Random.Range(1, 9);
                var txParams = new MintNft()
                {
                    Attributes = new Dictionary<string, object>()
                    {
                        { "nft_type", "face" },
                    },
                    Description = "You can use this as a face of your character in the game!",
                    MintCap = NFTMintCapIdField.text,
                    Recipient = TargetWalletAddressInputField.text,
                    ModuleName =  deadBytesType.Module,
                    Name = $"Face {randomFaceIndex}",
                    PackageObjectId = mintCapType.PackageId,
                    Signer = signerKeyPair.PublicKeyAsSuiAddress,
                    Url = $"https://suiunitysdksample.blob.core.windows.net/nfts/face{randomFaceIndex}.png"
                };

                // if we pass null, it will automatically select a gas object
                var mintRpcResult = await nftProtocolClient.MintNftAsync(txParams, null);
                if (mintRpcResult is { IsSuccess: true })
                {
                    // we query the top level nft object
                    var nftId = mintRpcResult.Result.Effects.Effects.Created.FirstOrDefault(c => c.Owner.Type == SuiOwnerType.AddressOwner)?.Reference.ObjectId;
                    if (!string.IsNullOrWhiteSpace(nftId))
                    {
                        var artNftResult = await SuiApi.NftProtocolClient.GetArtNftAsync(nftId);
                        if (artNftResult.IsSuccess)
                        {
                            await LoadNFT(artNftResult.Result.Url);
                            NFTMintedText.gameObject.SetActive(true);
                            NFTMintedReadonlyInputField.gameObject.SetActive(true);

                            NFTMintedReadonlyInputField.text = "https://explorer.devnet.sui.io/objects/" + nftId;
                        }
                        else
                        {
                            Debug.LogError("Something went wrong with the minting 3: " + artNftResult.ErrorMessage);
                        }
                    }
                    else
                    {
                        Debug.LogError("Something went wrong with the minting 2: " + mintRpcResult.RawRpcResponse);
                    }
                }
                else
                {
                    Debug.LogError("Something went wrong with the minting 1: " + mintRpcResult.ErrorMessage);
                }
            }
            else
            {
                Debug.LogError("Something went wrong with the minting 0: " + mintCapResult.ErrorMessage);
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
