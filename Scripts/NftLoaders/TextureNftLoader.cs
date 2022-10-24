using System.Threading.Tasks;
using Suinet.NftProtocol.Nft;
using UnityEngine;
using UnityEngine.Networking;

public class TextureNftLoader : MonoBehaviour
{
    public string NftObjectId;
    public string TargetTextureName;
    public Material TargetMaterial;

    void Awake()
    {
        TargetTextureName = "_MainTex";
    }
    
    // Start is called before the first frame update
    async void Start()
    {
        var getObjectRpcResult = await SuiApi.Client.GetObjectAsync<UniqueNft>(NftObjectId);

        if (getObjectRpcResult.IsSuccess)
        {
            await LoadNFT(getObjectRpcResult.Result.Data.Fields.Url);
        }
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
        TargetMaterial.SetTexture(TargetTextureName, tex);
    }
}
