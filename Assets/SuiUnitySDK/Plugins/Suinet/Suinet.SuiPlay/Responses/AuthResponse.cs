using Newtonsoft.Json;

namespace Suinet.SuiPlay.Responses
{
    public class AuthResponse
    {
        [JsonProperty("idToken")]
        public string IdToken { get; set; }
        
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
        
        [JsonProperty("expiresIn")]
        public string ExpiresIn { get; set; }
    }
}
