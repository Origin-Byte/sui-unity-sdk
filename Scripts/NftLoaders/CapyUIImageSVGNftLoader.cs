using Suinet.Rpc.Types.MoveTypes;
using UnityEngine;
using UnityEngine.UI;

public class CapyUIImageSVGNftLoader : MonoBehaviour {
    
    public string NftObjectId;

    private async void Start()
    {
        var capyObjectRpcResult = await SuiApi.Client.GetObjectAsync<CapyNft>(NftObjectId);

        if (!capyObjectRpcResult.IsSuccess) return;
        
        var url =  capyObjectRpcResult.Result.Url;
        var sceneInfo = await SVGHelper.LoadSVGAsync(url); 
        var image = gameObject.GetComponent<Image>();
        var sprite = SVGHelper.ConvertSVGToSprite(sceneInfo);
        image.sprite = sprite;
        image.useSpriteMesh = true;
    }
}