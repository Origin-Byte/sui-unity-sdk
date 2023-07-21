using Newtonsoft.Json;

namespace Suinet.SuiPlay.Responses
{
    public class RegistrationResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
