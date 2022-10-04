using Suinet.Rpc;
using Suinet.Rpc.Types;

public static class SuiApi
{
    public static IJsonRpcApiClient GatewayClient { get; private set; }
    public static IJsonRpcApiClient FullnodeClient { get; private set; }

    static SuiApi()
    {
        FullnodeClient = new SuiJsonRpcApiClient(new UnityWebRequestRpcClient(SuiConstants.DEVNET_FULLNODE_ENDPOINT));
        GatewayClient = new SuiJsonRpcApiClient(new UnityWebRequestRpcClient(SuiConstants.DEVNET_GATEWAY_ENDPOINT));
    }
}
