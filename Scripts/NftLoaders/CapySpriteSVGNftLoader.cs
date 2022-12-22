using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Unity.VectorGraphics;

public class CapySpriteSVGNftLoader : MonoBehaviour {
    
    public string NftObjectId;
    public SpriteSVGNftLoader AccessoryPrefab;
    
    private async void Start()
    {
        var capyObjectRpcResult = await SuiApi.Client.GetObjectAsync(NftObjectId);
        var accessoryObjectsRpcResult = await SuiApi.Client.GetObjectsOwnedByObjectAsync(NftObjectId);
        
        if (capyObjectRpcResult.IsSuccess)
        {
            var url =  capyObjectRpcResult.Result.Object.Data.Fields["url"] as string;
            var sceneInfo = await LoadSVGAsync(url);
            DrawSVG(sceneInfo);

            foreach (var accessoryDynamicObjectField in accessoryObjectsRpcResult.Result)
            {
                var dynamicObjectFieldResult = await SuiApi.Client.GetObjectAsync(accessoryDynamicObjectField.ObjectId);
                var dynamicObjectFieldId = dynamicObjectFieldResult.Result.Object.Data.Fields["value"] as string;
                var accessoryLoader = Instantiate(AccessoryPrefab, transform);
                accessoryLoader.NftObjectId = dynamicObjectFieldId;
                accessoryLoader.gameObject.SetActive(true);
            }
        }
    }
 
    public void DrawSVG(SVGParser.SceneInfo sceneInfo) {
        // Dynamically import the SVG data, and tessellate the resulting vector scene.
        var tesselationOptions = new VectorUtils.TessellationOptions()
        {
            StepDistance = 10f,
            MaxCordDeviation = 2f,
            SamplingStepSize = 0.5f,
            MaxTanAngleDeviation = float.MaxValue
        };
        var geoms = VectorUtils.TessellateScene(sceneInfo.Scene, tesselationOptions);
 
        // Build a sprite with the tessellated geometry.
        var sprite = VectorUtils.BuildSprite(geoms, 100.0f, VectorUtils.Alignment.SVGOrigin, Vector2.zero, 256, true);
        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
    }
 
    private async Task<SVGParser.SceneInfo> LoadSVGAsync(string url)
    {
        var svgText = await DownladSVGTextAsync(url);
        using (var reader = new StringReader(svgText))
        {
            return SVGParser.ImportSVG(reader);
        }
    }
    
    public async Task<string> DownladSVGTextAsync(string url)
    {
        var req = await UnityWebRequests.GetAsync(url);
        return req.downloadHandler.text;
    }

}