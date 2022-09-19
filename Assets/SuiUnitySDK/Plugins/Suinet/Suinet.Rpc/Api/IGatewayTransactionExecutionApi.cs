using Suinet.Rpc.Types;
using System.Threading.Tasks;

namespace Suinet.Rpc.Api
{
    public interface IGatewayTransactionExecutionApi
    {
        /// <summary>
        /// Execute the transaction using the transaction data, signature and public key.
        /// </summary>
        /// <param name="txBytes"></param>
        /// <param name="sigScheme"></param>
        /// <param name="signature"></param>
        /// <param name="pubKey"></param>
        /// <returns></returns>
        Task<RpcResult<SuiTransactionResponse>> ExecuteTransactionAsync(string txBytes, SuiSignatureScheme sigScheme, string signature, string pubKey);
    }
}
