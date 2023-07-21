using Newtonsoft.Json;

namespace Suinet.SuiPlay.Requests
{
    public class RegistrationRequest : LoginRequest
    {
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
    }
}
