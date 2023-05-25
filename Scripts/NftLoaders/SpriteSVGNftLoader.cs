using Suinet.Rpc.Types;
using Suinet.Rpc.Types.MoveTypes;
using UnityEngine;

public class SpriteSVGNftLoader : MonoBehaviour {
    public string NftObjectId;
    
    private async void Start()
    {
        var objectRpcResult = await SuiApi.Client.GetObjectAsync(NftObjectId, ObjectDataOptions.ShowAll());

        if (objectRpcResult.IsSuccess && objectRpcResult.Result.Data.Content is MoveObjectData moveContent)
        {
            var objMoveStruct = moveContent.Fields as ObjectMoveStruct;
            var url = (objMoveStruct.Fields["url"] as StringMoveValue).Value;
            var sceneInfo = await SVGHelper.LoadSVGAsync(url);
            gameObject.GetComponent<SpriteRenderer>().sprite = SVGHelper.ConvertSVGToSprite(sceneInfo);
        }
    }
}