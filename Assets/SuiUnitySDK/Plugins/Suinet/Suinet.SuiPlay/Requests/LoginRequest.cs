using Newtonsoft.Json;

namespace Suinet.SuiPlay.Requests
{
    public class LoginRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        
        [JsonProperty("password")]
        public string Password { get; set; }
        
        [JsonProperty("studioId")]
        public string StudioId { get; set; }
        
        [JsonProperty("gameId")]
        public string GameId { get; set; }
    }
}
