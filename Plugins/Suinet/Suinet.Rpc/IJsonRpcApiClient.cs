using Suinet.Rpc.Api;

namespace Suinet.Rpc
{
    public interface IJsonRpcApiClient : IReadApi, ITransactionBuilderApi, IGatewayTransactionExecutionApi, IEventReadApi
    {
    }
}
