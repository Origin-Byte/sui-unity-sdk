using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json;
using Suinet.SuiPlay.Requests;
using Suinet.NftProtocol;
using Suinet.NftProtocol.Examples;
using Suinet.Rpc.Types;

public class SuiPlayMintUIController : MonoBehaviour
{
    public Button actionButton;
    public TMP_InputField packageId;
    public TMP_InputField walletId;
    public TMP_InputField output; // outputs can be copy-pasted

    private async Task Start()
    {
        packageId.text = "0xb11eda772add7178d97d98fbcb5dc73ea1afec0bb94705416c43efdbedba6e4b";
        var player = await SuiPlay.Client.GetPlayerProfileAsync(SuiPlayConfig.GAME_ID);
        var firstPlayerWallet = player.Value.Wallets.First().Value;
        walletId.text = firstPlayerWallet.Id;
        
        actionButton.onClick.AddListener(async () =>
        {
            var randomFaceIndex = Random.Range(1, 9);
            var txBuilder = new MintSuitradersNft()
            {
                Attributes = new Dictionary<string, object>()
                {
                    { "nft_type", "face" },
                },
                Description = "You can use this as a face of your character in the game!",
                Recipient = firstPlayerWallet.Address,
                ModuleName =  "suitraders",
                Function = "airdrop_nft",
                Name = $"Face {randomFaceIndex}",
                PackageObjectId = packageId.text,
                Signer = firstPlayerWallet.Address,
                Url = $"https://suiunitysdksample.blob.core.windows.net/nfts/face{randomFaceIndex}.png"
            };
            var moveCallResult = await SuiApi.Client.MoveCallAsync(txBuilder.BuildMoveCallTransaction(null));
            var txBytes = moveCallResult.Result.TxBytes;
            var request = new SignTransactionRequest()
            {
                TxBytes = txBytes
            };
            var signatureResult = await SuiPlay.Client.SignTransactionAsync(SuiPlayConfig.GAME_ID, walletId.text, request);
            var txResult = await SuiApi.Client.ExecuteTransactionBlockAsync(txBytes, new[] { signatureResult.Value.Signature.Signature }, TransactionBlockResponseOptions.ShowAll(), ExecuteTransactionRequestType.WaitForLocalExecution);
            
            output.text = JsonConvert.SerializeObject(txResult, Formatting.Indented);
        });
    }
}
