using Suinet.NftProtocol.TransactionBuilders;
using Suinet.Rpc.Types;
using Suinet.Rpc;
using System.Threading.Tasks;

namespace Suinet.NftProtocol
{
    public interface INftProtocolClient
    {
        Task<RpcResult<SuiExecuteTransactionResponse>> MintNftAsync(MintNftToLaunchpad txParams, string gasObjectId = null);

        Task<RpcResult<SuiExecuteTransactionResponse>> EnableSalesAsync(EnableSales txParams, string gasObjectId = null);

        Task<RpcResult<SuiExecuteTransactionResponse>> BuyNftCertificateAsync
(BuyNftCertificate txParams, string gasObjectId = null);

        Task<RpcResult<SuiExecuteTransactionResponse>> CaimNftCertificateAsync
(ClaimNftCertificate txParams, string gasObjectId = null);
    }
}
