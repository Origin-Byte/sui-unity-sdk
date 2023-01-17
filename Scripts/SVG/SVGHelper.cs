using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Unity.VectorGraphics;

public static class SVGHelper
{
    public static Sprite ConvertSVGToSprite(SVGParser.SceneInfo sceneInfo) {
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
        return VectorUtils.BuildSprite(geoms, 100.0f, VectorUtils.Alignment.Center, new Vector2(0.5f, 0.5f), 256, true);
    }

    public static IEnumerator LoadSVG(string url, Action<SVGParser.SceneInfo> onSceneInfoReady)
    {
        yield return DownloadSVGText(url,svgText =>
        {
            var sceneInfo = ParseSVG(svgText);
            onSceneInfoReady(sceneInfo);
        });
    }
    
    public static async Task<SVGParser.SceneInfo> LoadSVGAsync(string url)
    {
        var svgText = await DownladSVGTextAsync(url);
        return ParseSVG(svgText);
    }

    private static SVGParser.SceneInfo ParseSVG(string svgText)
    {
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

        using var reader = new StringReader(doc.ToString());
        return SVGParser.ImportSVG(reader);
    }

    private static async Task<string> DownladSVGTextAsync(string url)
    {
        var req = await UnityWebRequests.GetAsync(url);
        return req.downloadHandler.text;
    }

    private static IEnumerator DownloadSVGText(string url, Action<string> onTextResultReady)
    {
        yield return UnityWebRequests.Get(url, req =>
        {
            var data = req.downloadHandler.text;
            onTextResultReady(data);
        });
    }
}
