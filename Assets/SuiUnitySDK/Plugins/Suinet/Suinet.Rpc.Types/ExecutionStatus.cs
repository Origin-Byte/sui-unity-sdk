using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace Suinet.Rpc.Types
{
    public enum ExecutionStatus
    {
        Success,
        Failure,
    }

    public class ExecutionStatusResponse
    {
        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ExecutionStatus Status { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }
}
