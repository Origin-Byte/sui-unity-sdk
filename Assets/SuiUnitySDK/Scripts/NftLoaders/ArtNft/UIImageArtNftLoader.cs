using System.Threading.Tasks;
using Suinet.NftProtocol.Nft;
using Unity.Plastic.Newtonsoft.Json;
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
            Debug.Log( "getObjectRpcResult: " + JsonConvert.SerializeObject(getObjectRpcResult));
            await LoadNFTAsync(getObjectRpcResult.Result.Url);
        }
    }
    
    public async Task LoadNFTAsync(string url)
    {
        if (url.StartsWith("ipfs://"))
        {
            string ipfsHash = url.Substring(7);
            url = $"https://ipfs.io/ipfs/{ipfsHash}";
        }
        
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
