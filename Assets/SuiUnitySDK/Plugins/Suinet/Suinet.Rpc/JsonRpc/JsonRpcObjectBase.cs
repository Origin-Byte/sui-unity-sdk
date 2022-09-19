using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Suinet.Rpc.JsonRpc
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Ignore)]
    public abstract class JsonRpcObjectBase
    {
        public string Jsonrpc
        {
            get
            {
                return "2.0";
            }
        }

        public int Id { get; set; }
    }
}
