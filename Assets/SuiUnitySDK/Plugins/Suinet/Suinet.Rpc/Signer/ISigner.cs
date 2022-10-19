using Suinet.Rpc.Types;
using System;
using System.Threading.Tasks;

namespace Suinet.Rpc.Signer
{
    public interface ISigner
    {
        Task<RpcResult<SuiExecuteTransactionResponse>> SignAndExecuteAsync(Func<Task<RpcResult<SuiTransactionBytes>>> method, SuiExecuteTransactionRequestType txRequestType);

        Task<RpcResult<SuiExecuteTransactionResponse>> SignAndExecuteMoveCallAsync(MoveCallTransaction moveCallTransaction);
    }
}
