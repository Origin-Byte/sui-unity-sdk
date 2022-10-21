using System.Threading.Tasks;
using Suinet.NftProtocol.Nft;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIImageNftLoader : MonoBehaviour
{
    public string NftObjectId;
    public Image NFTImage;
    
    // Start is called before the first frame update
    async void Start()
    {
        var getObjectRpcResult = await SuiApi.Client.GetObjectAsync<UniqueNft>(NftObjectId);

        if (getObjectRpcResult.IsSuccess)
        {
            await LoadNFT(getObjectRpcResult.Result.Data.Fields.Url);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
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
