using Suinet.Rpc.Types;
using System;
using System.Threading.Tasks;

namespace Suinet.Rpc.Signer
{
    public interface ISigner
    {
        Task<RpcResult<TransactionBlockResponse>> SignAndExecuteAsync(Func<Task<RpcResult<TransactionBlockBytes>>> method, ExecuteTransactionRequestType txRequestType);

        //Task<RpcResult<TransactionBlockResponse>> SignAndExecuteMoveCallAsync(MoveCallTransaction moveCallTransaction);
    }
}
