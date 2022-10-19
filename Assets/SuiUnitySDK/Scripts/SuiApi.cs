using Suinet.Rpc;
using Suinet.Rpc.Client;
using Suinet.Rpc.Types;

public static class SuiApi
{
    public static IJsonRpcApiClient Client { get; private set; }

    static SuiApi()
    {
        var rpcClient = new UnityWebRequestRpcClient(SuiConstants.DEVNET_ENDPOINT);
        Client = new SuiJsonRpcApiClient(rpcClient);
    }
}
