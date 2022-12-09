using System.Threading.Tasks;
using Suinet.NftProtocol.Nft;
using UnityEngine;
using UnityEngine.UI;

public class UIImageFilteredNftLoaderForAddress : MonoBehaviour
{
    public string Address;
    public Image[] Images;
    public string FilterAttributeName;
    public string FilterAttributeValue;
    
    async void Start()
    {
        var getObjectRpcResult = await SuiApi.Client.GetObjectsOwnedByAddressAsync<UniqueNft>(Address);

        if (getObjectRpcResult.IsSuccess)
        {
            int i = 0;
            foreach (var nftData in getObjectRpcResult.Result)
            {
                var attributes = nftData.Data.Fields.Attributes.Fields.ToDictionary();

                if (!attributes.ContainsKey(FilterAttributeName) ||
                    attributes[FilterAttributeName] != FilterAttributeValue)
                {
                    continue;;
                }
                
                if (Images.Length > i)
                {
                    await LoadNFTsAsync(nftData.Data.Fields.Url, Images[i]);
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
    public async Task LoadNFTsAsync(string url, Image nftImage)
    {
        var req = await UnityWebRequests.GetAsync(url);
        var data = req.downloadHandler.data;
        SetSpriteFromData(data, nftImage);
    }
    
    private void SetSpriteFromData(byte[] data, Image nftImage)
    {
        var tex = new Texture2D((int)nftImage.preferredWidth, (int)nftImage.preferredHeight);
        tex.LoadImage(data);
        var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
        nftImage.sprite = sprite;
    }
}
