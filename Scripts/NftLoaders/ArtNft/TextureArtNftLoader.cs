using System.Threading.Tasks;
using Suinet.NftProtocol.Nft;
using Suinet.Rpc.Types.MoveTypes;
using UnityEngine;

public class TextureArtNftLoader : MonoBehaviour
{
    public string NftObjectId;
    public string TargetTextureName;
    public Renderer TargetRenderer;

    void Awake()
    {
        TargetTextureName = "_MainTex";
    }
    
    // Start is called before the first frame update
    async void Start()
    {
        var getObjectRpcResult = await SuiApi.NftProtocolClient.GetArtNftAsync(NftObjectId);

        if (getObjectRpcResult.IsSuccess)
        {
            LoadNFT(getObjectRpcResult.Result.Url);
        }
    }

    public async Task LoadNFTAsync(string url)
    {
        var req = await UnityWebRequests.GetAsync(url);
        var data = req.downloadHandler.data;
        SetTextureFromData(data);
    }

    public void LoadNFT(string url)
    {
        StartCoroutine(UnityWebRequests.Get(url, onSuccess: req =>
        {
            var data = req.downloadHandler.data;
            SetTextureFromData(data);
        }));
    }

    private void SetTextureFromData(byte[] data)
    {
        var tex = new Texture2D(256, 256);
        tex.LoadImage(data);
        TargetRenderer.material.SetTexture(TargetTextureName, tex);
    }
}
