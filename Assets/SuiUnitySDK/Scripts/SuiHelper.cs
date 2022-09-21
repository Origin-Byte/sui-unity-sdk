using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Suinet.Rpc;
using Suinet.Rpc.Types;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// TODO: refactor this form static helper
public static class SuiHelper
{
    public static async Task<List<string>> GetCoinObjectIdsOwnedByAddressAsync(string address, string coinType = "0x2::coin::Coin<0x2::sui::SUI>")
    {
        var rpcClient = new UnityWebRequestRpcClient(SuiConstants.DEVNET_ENDPOINT);
        var suiJsonRpcApi = new SuiJsonRpcApiClient(rpcClient);

        var ownedObjectsResult = await suiJsonRpcApi.GetObjectsOwnedByAddressAsync(address);

        return ownedObjectsResult.Result.Where(r => r.Type == coinType).Select(c => c.ObjectId).ToList();
    }

    
    public static async Task<List<string>> GetCoinObjectIdsAboveBalancesOwnedByAddressAsync(string address, int count = 10, ulong minBalance = 1000, string coinType = "0x2::coin::Coin<0x2::sui::SUI>")
    {
        var rpcClient = new UnityWebRequestRpcClient(SuiConstants.DEVNET_ENDPOINT);
        var suiJsonRpcApi = new SuiJsonRpcApiClient(rpcClient);

        var coinObjectIds = await GetCoinObjectIdsOwnedByAddressAsync(address, coinType);

        var result = new List<string>();

        foreach(var coinObjectId in coinObjectIds)
        {
            var objectResult = await suiJsonRpcApi.GetObjectAsync(coinObjectId);
            var jObj = JObject.FromObject(objectResult.Result.Details);
            var coinBalance = jObj.SelectToken("data.fields['balance']").Value<ulong>();

            if (coinBalance > minBalance)
            {
                result.Add(coinObjectId);
            }

            if (result.Count >= count)
            {
                break;
            }
        }
        return result;
    }
}
