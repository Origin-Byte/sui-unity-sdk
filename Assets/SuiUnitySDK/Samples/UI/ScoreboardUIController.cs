using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Suinet.Rpc;
using Suinet.Rpc.Types;
using UnityEngine;

public class ScoreboardUIController : MonoBehaviour
{
    public ScoreboardElement scoreboardElementPrefab;
    public Transform scoreboardElementsParent;

    private IJsonRpcApiClient _gatewayClient;

    public async void Start()
    {
        _gatewayClient = new SuiJsonRpcApiClient(new UnityWebRequestRpcClient(SuiConstants.DEVNET_GATEWAY_ENDPOINT));
        await LoadScoreboardAsync();
    }

    public async Task LoadScoreboardAsync()
    {
        scoreboardElementsParent.Clear();
        var rpcResult = await _gatewayClient.GetObjectAsync(Constants.SCOREBOARD_OBJECT_ID);

        if (rpcResult.IsSuccess)
        {
            var detailsObject = JObject.FromObject(rpcResult.Result.Details);
            var scores = detailsObject["data"]["fields"]["scores"] as JArray;
            var scoreboardElements = scores.ToObject<ScoreboardMoveType[]>();

            foreach (var element in scoreboardElements)
            {
                var elementGo = Instantiate(scoreboardElementPrefab, scoreboardElementsParent);
                elementGo.UpdateElementData(element.Fields.Player, element.Fields.Score);
                elementGo.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("Could not get scoreboard object with id " + Constants.SCOREBOARD_OBJECT_ID);
        }
    }

    public class ScoreboardMoveType
    {
        public string Type { get; set; }
        public ScoreboardElementFields Fields { get; set; }
    }
    
    public class ScoreboardElementFields
    {
        public string Player { get; set; }
        
        public ulong Score { get; set; }
    }
}
