using UnityEngine;

public class SpriteSVGNftLoader : MonoBehaviour {
    public string NftObjectId;
    
    private async void Start()
    {
        var objectRpcResult = await SuiApi.Client.GetObjectAsync(NftObjectId);
        
        if (objectRpcResult.IsSuccess)
        {
            var url =  objectRpcResult.Result.Object.Data.Fields["url"] as string;
            var sceneInfo = await SVGHelper.LoadSVGAsync(url);
            SVGHelper.DrawSVGAsSprite(sceneInfo, gameObject.GetComponent<SpriteRenderer>());
        }
    }
}