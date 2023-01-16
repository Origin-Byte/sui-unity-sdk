using System.Threading.Tasks;
using Suinet.NftProtocol.Nft;
using UnityEngine;
using UnityEngine.UI;

public class UIImageArtNftLoader : MonoBehaviour
{
    public string NftObjectId;
    public Image NFTImage;
    
    // Start is called before the first frame update
    async void Start()
    {
        var getObjectRpcResult = await SuiApi.NftProtocolClient.GetArtNftAsync(NftObjectId);

        if (getObjectRpcResult.IsSuccess)
        {
            await LoadNFTAsync(getObjectRpcResult.Result.Url);
        }
    }
    
    public async Task LoadNFTAsync(string url)
    {
        var req = await UnityWebRequests.GetAsync(url);
        var data = req.downloadHandler.data;
        SetSpriteFromData(data);
    }
    
    public void LoadNFT(string url)
    {
        UnityWebRequests.Get(url, req =>
        {
            var data = req.downloadHandler.data;
            SetSpriteFromData(data);
        });
    }
    
    private void SetSpriteFromData(byte[] data)
    {
        var tex = new Texture2D((int)NFTImage.preferredWidth, (int)NFTImage.preferredHeight);
        tex.LoadImage(data);
        var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
        NFTImage.sprite = sprite;
    }
}
