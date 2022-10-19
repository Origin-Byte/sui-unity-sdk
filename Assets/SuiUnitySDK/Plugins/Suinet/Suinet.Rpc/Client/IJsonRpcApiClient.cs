using Suinet.Rpc.Api;

namespace Suinet.Rpc.Client
{
    public interface IJsonRpcApiClient : IReadApi, ITransactionBuilderApi, IQuorumDriverApi, IEventReadApi, IGenericReadApi
    {
    }
}
