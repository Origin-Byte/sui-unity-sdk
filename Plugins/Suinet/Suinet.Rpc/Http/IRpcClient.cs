using Suinet.Rpc.JsonRpc;
using System;
using System.Threading.Tasks;

namespace Suinet.Rpc.Http
{
    public interface IRpcClient
    {
        Uri Endpoint { get; }
        Task<RpcResult<T>> SendAsync<T>(JsonRpcRequest request);
    }
}
