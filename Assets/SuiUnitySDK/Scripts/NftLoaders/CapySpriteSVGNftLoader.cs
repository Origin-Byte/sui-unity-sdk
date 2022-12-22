using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using System.Xml.Linq;
using Unity.VectorGraphics;

public class CapySpriteSVGNftLoader : MonoBehaviour {
    
    public string NftObjectId;
    public SpriteSVGNftLoader AccessoryPrefab;
    public bool LoadAccessories = false;
    
    private async void Start()
    {
        var capyObjectRpcResult = await SuiApi.Client.GetObjectAsync(NftObjectId);
        var accessoryObjectsRpcResult = await SuiApi.Client.GetObjectsOwnedByObjectAsync(NftObjectId);
        
        if (capyObjectRpcResult.IsSuccess)
        {
            var url =  capyObjectRpcResult.Result.Object.Data.Fields["url"] as string;
            var sceneInfo = await LoadSVGAsync(url);
            DrawSVG(sceneInfo);

            if (LoadAccessories)
            {
                foreach (var accessoryDynamicObjectField in accessoryObjectsRpcResult.Result)
                {
                    var dynamicObjectFieldResult =
                        await SuiApi.Client.GetObjectAsync(accessoryDynamicObjectField.ObjectId);
                    var dynamicObjectFieldId = dynamicObjectFieldResult.Result.Object.Data.Fields["value"] as string;
                    var accessoryLoader = Instantiate(AccessoryPrefab, transform);
                    accessoryLoader.NftObjectId = dynamicObjectFieldId;
                    accessoryLoader.gameObject.SetActive(true);
                }
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
        
        // xml cleanup
        var doc = XDocument.Parse(svgText);
        var svgElement = doc.Elements().First();
        var style = "";
        var styleElements = new List<XElement>();
        foreach (var element in svgElement.Elements())
        {
            if (element.Name == "{http://www.w3.org/2000/svg}style")
            {
                style += element.Value;
                style += Environment.NewLine;
                
                styleElements.Add(element);
            }
        }

        var firstStyleElement = styleElements.First();
        firstStyleElement.Value = style;
        styleElements.Remove(firstStyleElement);
        foreach (var se in styleElements)
        {
            se.Remove();
        }

        using (var reader = new StringReader(doc.ToString()))
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