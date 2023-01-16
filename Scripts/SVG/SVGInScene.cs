using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Unity.VectorGraphics;

public class SVGInScene : MonoBehaviour {
    public TextAsset svgAsset;
 
    public void Start() {
        initSVG();
    }
 
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
 
        // Build a sprite with the tessellated geometry.
        var sprite = VectorUtils.BuildSprite(geoms, 1.0f, VectorUtils.Alignment.Center, Vector2.zero, 256, true);
        sprite.name = svgAsset.name;
        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
    }
 
    private SVGParser.SceneInfo loadSVG() {
        using (var reader = new StringReader(svgAsset.text)) { // not strictly needed but in case switch later.
            return SVGParser.ImportSVG(reader);
        }
    }
}