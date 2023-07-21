namespace Suinet.SuiPlay.Responses
{
    public class AuthResponse
    {
        public string IdToken { get; set; }
        public string RefreshToken { get; set; }
        public string ExpiresIn { get; set; }
    }
}
