using Suinet.Rpc.Types.MoveTypes;
using Suinet.Rpc.Types.Nfts;
using Suinet.Rpc.Types.ObjectDataParsers;
using UnityEngine;

public class CapySpriteSVGNftLoader : MonoBehaviour {
    
    public string NftObjectId;
    public SpriteSVGNftLoader AccessoryPrefab;
    public bool LoadAccessories = false;
    
    private async void Start()
    {
        var capyObjectRpcResult = await SuiApi.Client.GetObjectAsync<CapySuiFren>(NftObjectId, new CapySuiFrenParser());

        if (!capyObjectRpcResult.IsSuccess) return;
        
        var url =  capyObjectRpcResult.Result.Display.ImageUrl;
        var sceneInfo = await SVGHelper.LoadSVGAsync(url);
        gameObject.GetComponent<SpriteRenderer>().sprite = SVGHelper.ConvertSVGToSprite(sceneInfo);

        if (!LoadAccessories) return;
        
        // var accessoryObjectsRpcResult = await SuiApi.Client.GetObjectsOwnedByObjectAsync(NftObjectId);
        // foreach (var accessoryDynamicObjectField in accessoryObjectsRpcResult.Result)
        // {
        //     var dynamicObjectFieldResult =
        //         await SuiApi.Client.GetObjectAsync(accessoryDynamicObjectField.ObjectId);
        //     var dynamicObjectFieldId = dynamicObjectFieldResult.Result.Object.Data.Fields["value"] as string;
        //     var accessoryLoader = Instantiate(AccessoryPrefab, transform);
        //     accessoryLoader.NftObjectId = dynamicObjectFieldId;
        //     accessoryLoader.gameObject.SetActive(true);
        // }
    }
}