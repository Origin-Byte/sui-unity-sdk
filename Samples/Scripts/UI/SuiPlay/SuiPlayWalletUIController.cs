using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json;
using Suinet.NftProtocol.Nft;
using Suinet.Rpc.Types;
using Suinet.SuiPlay.Requests;

public class SuiPlayWalletUIController : MonoBehaviour
{
    public Button actionButton;
    public TMP_InputField output; // outputs can be copy-pasted
    public TMP_InputField coinBalance; 

    private void Start()
    {
        actionButton.onClick.AddListener(async () =>
        {
            var playerProfile = await SuiPlay.Client.GetPlayerProfileAsync(SuiPlayConfig.GAME_ID);

            var walletAddress = playerProfile.Value.Wallets.First().Value.Address;
            var coinBalanceResult = await SuiApi.Client.GetBalanceAsync(walletAddress, null);
            coinBalance.text = coinBalanceResult.Result.TotalBalance.ToString();

            // var nftsResult = await SuiApi.Client.GetObjectsOwnedByAddressAsync(walletAddress, new ArtNftParser(),
            //     options: ObjectDataOptions.ShowAll());
            var filter = ObjectDataFilterFactory.CreateMatchAllFilter(ObjectDataFilterFactory.CreateAddressOwnerFilter(walletAddress));
            var ownedObjectsResult = await SuiApi.Client.GetOwnedObjectsAsync(walletAddress,
                new ObjectResponseQuery() { Filter = filter }, null, null);

            output.text = JsonConvert.SerializeObject(coinBalanceResult.Result, Formatting.Indented) + "\n" + JsonConvert.SerializeObject(ownedObjectsResult.Result, Formatting.Indented);
        });
    }
}
