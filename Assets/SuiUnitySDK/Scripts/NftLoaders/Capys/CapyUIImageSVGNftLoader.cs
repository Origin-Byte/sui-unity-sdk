using Suinet.Rpc.Types.MoveTypes;
using Suinet.Rpc.Types.Nfts;
using Suinet.Rpc.Types.ObjectDataParsers;
using UnityEngine;
using UnityEngine.UI;

public class CapyUIImageSVGNftLoader : MonoBehaviour {
    
    public string NftObjectId;

    private async void Start()
    {
        var capyObjectRpcResult = await SuiApi.Client.GetObjectAsync<CapySuiFren>(NftObjectId, new CapySuiFrenParser());

        if (!capyObjectRpcResult.IsSuccess) return;
        
        var url =  capyObjectRpcResult.Result.Display.ImageUrl;
        var sceneInfo = await SVGHelper.LoadSVGAsync(url); 
        var image = gameObject.GetComponent<Image>();
        var sprite = SVGHelper.ConvertSVGToSprite(sceneInfo);
        image.sprite = sprite;
        image.useSpriteMesh = true;
    }
}