using System.Linq;
using System.Threading.Tasks;
using Suinet.NftProtocol.Nft;
using UnityEngine;
using UnityEngine.UI;

public class UIImageFilteredArtNftLoaderForAddress : MonoBehaviour
{
    public string Address;
    public Image ImagePrefab;
    public string FilterAttributeName;
    public string FilterAttributeValue;
    
    async void Start()
    {
        var getObjectRpcResult = await SuiApi.NftProtocolClient.GetArtNftsOwnedByAddressAsync(Address);

        Debug.Log("getObjectRpcResult cnt: " + getObjectRpcResult?.Result?.Count());
        if (getObjectRpcResult.IsSuccess)
        {
            //int i = 0;
            foreach (var nftData in getObjectRpcResult.Result)
            {
                // var attributes = nftData.Attributes;
                //
                // if (attributes == null || !attributes.ContainsKey(FilterAttributeName) ||
                //     !attributes[FilterAttributeName].Contains(FilterAttributeValue))
                // {
                //     continue;
                // }
                
                var imageGo = Instantiate(ImagePrefab, transform);
                await LoadNFTsAsync(nftData.Url, imageGo);
                imageGo.gameObject.SetActive(true);
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
