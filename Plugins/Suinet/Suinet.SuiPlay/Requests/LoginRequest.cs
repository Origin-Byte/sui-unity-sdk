namespace Suinet.SuiPlay.Requests
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string StudioId { get; set; }
        public string GameId { get; set; }
    }
}
