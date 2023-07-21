using Suinet.Rpc.Types;
using Suinet.SuiPlay;

public static class SuiPlay
{
    public static GameClientApiClient Client { get; }
    
    static SuiPlay()
    {
        var httpService = new UnityHttpService(SuiConstants.SUIPLAY_API_URL);
        Client = new GameClientApiClient(httpService, new UnityTokenStorage());
    }
}
