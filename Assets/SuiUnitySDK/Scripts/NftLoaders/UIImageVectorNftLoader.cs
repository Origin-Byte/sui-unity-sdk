using System;
using System.IO;
using System.Threading.Tasks;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class UIImageVectorNftLoader : MonoBehaviour
{
    public string NftObjectId;
    public Image NFTImage;
    
    // Start is called before the first frame update
    async void Start()
    {
        //var getObjectRpcResult = await SuiApi.Client.GetObjectAsync<ArtNft>(NftObjectId);

        //if (getObjectRpcResult.IsSuccess)
        {
            //await LoadNFTAsync(getObjectRpcResult.Result.Data.Fields.Url);
            initSVG();
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
    
    public TextAsset svgAsset;
 
    private void initSVG() {
        // Dynamically import the SVG data, and tessellate the resulting vector scene.
        var sceneInfo = loadSVG();
        var tesselationOptions = new VectorUtils.TessellationOptions()
        {
            StepDistance = 10f,
            MaxCordDeviation = 2f,
            SamplingStepSize = 0.5f,
            MaxTanAngleDeviation = float.MaxValue
        };
        var geoms = VectorUtils.TessellateScene(sceneInfo.Scene, tesselationOptions);
 
        var vectorSprite = VectorUtils.BuildSprite(geoms, 100.0f, VectorUtils.Alignment.Center, Vector2.zero, 64, true);
       // var text2d = VectorUtils.RenderSpriteToTexture2D(vectorSprite, (int) NFTImage.preferredWidth,
         //   (int) NFTImage.preferredHeight, NFTImage.material);

//        var sprite = Sprite.Create(text2d, new Rect(0, 0, text2d.width, text2d.height), new Vector2(text2d.width / 2, text2d.height / 2));
        NFTImage.sprite = vectorSprite;
    }
 
    private SVGParser.SceneInfo loadSVG() {
        using (var reader = new StringReader(svgAsset.text)) { // not strictly needed but in case switch later.
            return SVGParser.ImportSVG(reader, 101);
        }
    }
}
