using UnityEngine;

public class CapySpriteSVGNftLoader : MonoBehaviour {
    
    public string NftObjectId;
    public SpriteSVGNftLoader AccessoryPrefab;
    public bool LoadAccessories = false;
    
    private async void Start()
    {
        var capyObjectRpcResult = await SuiApi.Client.GetObjectAsync(NftObjectId);

        if (!capyObjectRpcResult.IsSuccess) return;
        
        var url =  capyObjectRpcResult.Result.Object.Data.Fields["url"] as string;
        var sceneInfo = await SVGHelper.LoadSVGAsync(url);
        SVGHelper.DrawSVGAsSprite(sceneInfo, gameObject.GetComponent<SpriteRenderer>());

        if (!LoadAccessories) return;
        
        var accessoryObjectsRpcResult = await SuiApi.Client.GetObjectsOwnedByObjectAsync(NftObjectId);
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