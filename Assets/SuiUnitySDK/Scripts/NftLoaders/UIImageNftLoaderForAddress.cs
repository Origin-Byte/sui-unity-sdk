using System.Threading.Tasks;
using Suinet.NftProtocol.Nft;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UIImageNftLoaderForAddress : MonoBehaviour
{
    public string Address;
    public Image[] Images;
    
    async void Start()
    {
        var getObjectRpcResult = await SuiApi.Client.GetObjectsOwnedByAddressAsync<UniqueNft>(Address);

        if (getObjectRpcResult.IsSuccess)
        {
            int i = 0;
            foreach (var nftData in getObjectRpcResult.Result)
            {
                if (Images.Length > i)
                {
                    await LoadNFT(nftData.Data.Fields.Url, Images[i]);
                }
                
                i++;
            }

            while (i < Images.Length)
            {
                Images[i].gameObject.SetActive(false);
                i++;
            }
        }
    }
    private async Task LoadNFT(string url, Image NFTImage)
    {
        using var req = new UnityWebRequest(url, "GET");

        req.downloadHandler = new DownloadHandlerBuffer();
        req.SendWebRequest();
        while (!req.isDone)
        {
            await Task.Yield();
        }
        var data = req.downloadHandler.data;

        var tex = new Texture2D((int)NFTImage.preferredWidth, (int)NFTImage.preferredHeight);
        tex.LoadImage(data);
        var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
        NFTImage.sprite = sprite;
    }
}
