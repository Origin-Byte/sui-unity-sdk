namespace Suinet.Rpc
{
    using Newtonsoft.Json.Linq;
    using Suinet.Rpc.Types;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    // Convenience methods, built on raw sui json rpc methods
    public static class SuiHelper
    {
        public static async Task<List<string>> GetCoinObjectIdsOwnedByAddressAsync(IJsonRpcApiClient client, string address, string coinType = SuiConstants.SUI_COIN_TYPE)
        {
            var ownedObjectsResult = await client.GetObjectsOwnedByAddressAsync(address);
            return ownedObjectsResult.Result.Where(r => r.Type == coinType).Select(c => c.ObjectId).ToList();
        }

        public static async Task<List<string>> GetCoinObjectIdsAboveBalancesOwnedByAddressAsync(IJsonRpcApiClient client, string address, int count = 10, ulong minBalance = 1000, string coinType = SuiConstants.SUI_COIN_TYPE)
        {
            var coinObjectIds = await GetCoinObjectIdsOwnedByAddressAsync(client, address, coinType);

            var result = new List<string>();

            foreach (var coinObjectId in coinObjectIds)
            {
                var objectResult = await client.GetObjectAsync(coinObjectId);
                if (objectResult.IsSuccess)
                {
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
            }
            return result;
        }
    }

}
