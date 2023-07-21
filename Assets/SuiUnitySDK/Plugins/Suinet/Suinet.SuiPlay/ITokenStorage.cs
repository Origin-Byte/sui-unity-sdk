namespace Suinet.SuiPlay
{
    public interface ITokenStorage
    {
        void SaveToken(string token);
        string LoadToken();
    }
}