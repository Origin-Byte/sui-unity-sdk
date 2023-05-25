using Suinet.Rpc.Api;

namespace Suinet.Rpc.Client
{
    public interface IJsonRpcApiClient : IReadApi, IWriteApi, IExtendedApi, IGenericReadApi, ICoinQueryApi, ITransactionBuilderApi
    {
    }
}
