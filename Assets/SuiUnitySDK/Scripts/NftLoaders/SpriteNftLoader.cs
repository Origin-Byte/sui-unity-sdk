using System.Threading.Tasks;
using Suinet.Rpc.Types.MoveTypes;
using Suinet.Rpc.Types.Nfts;
using Suinet.Rpc.Types.ObjectDataParsers;
using UnityEngine;

/// <summary>
/// Load Origin Byte and Capy NFTs
/// </summary>
public class SpriteNftLoader : MonoBehaviour
{ 
    public string NftObjectId;
    public SpriteRenderer TargetRenderer;

    // Start is called before the first frame update
    async void Start()
    {
        var capyObjectRpcResult = await SuiApi.Client.GetObjectAsync<CapySuiFren>(NftObjectId, new CapySuiFrenParser());

        TargetRenderer.enabled = false;

        if (capyObjectRpcResult.IsSuccess && !string.IsNullOrWhiteSpace(capyObjectRpcResult.Result.Display.ImageUrl))
        {
            var sceneInfo = await SVGHelper.LoadSVGAsync(capyObjectRpcResult.Result.Display.ImageUrl);
            TargetRenderer.sprite = SVGHelper.ConvertSVGToSprite(sceneInfo);
            transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            TargetRenderer.enabled = true;
        }
        else
        {
            var getObjectRpcResult = await SuiApi.NftProtocolClient.GetArtNftAsync(NftObjectId);

            if (getObjectRpcResult.IsSuccess)
            {
                transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

                LoadNFT(getObjectRpcResult.Result.Url);
            }  
        }
    }

    public async Task LoadNFTAsync(string url)
    {
        var req = await UnityWebRequests.GetAsync(url);
        var data = req.downloadHandler.data;
        SetSpriteTextureFromData(data);
    }

    public void LoadNFT(string url)
    {
        StartCoroutine(UnityWebRequests.Get(url, onSuccess: req =>
        {
            var data = req.downloadHandler.data;
            SetSpriteTextureFromData(data);
        }));
    }

    private void SetSpriteTextureFromData(byte[] data)
    {
        var tex = new Texture2D(512, 512);
        tex.LoadImage(data);
        TargetRenderer.sprite = Sprite.Create(tex, new Rect(0, 0, 512, 512 ), new Vector2(0.5f, 0.5f),100f);
        TargetRenderer.enabled = true;
    }
}
