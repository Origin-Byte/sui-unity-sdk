using Suinet.NftProtocol.TransactionBuilders;
using Suinet.Rpc;
using Suinet.Rpc.Client;
using Suinet.Rpc.Signer;
using Suinet.Rpc.Types;
using System.Threading.Tasks;

namespace Suinet.NftProtocol
{
    public class NftProtocolClient : INftProtocolClient
    {
        private readonly IJsonRpcApiClient _jsonRpcApiClient;
        private readonly ISigner _signer;

        public NftProtocolClient(IJsonRpcApiClient jsonRpcApiClient, ISigner signer)
        {
            _jsonRpcApiClient = jsonRpcApiClient;
            _signer = signer;
        }

        public async Task<RpcResult<SuiExecuteTransactionResponse>> MintNftAsync(MintNftToLaunchpad txParams, string gasObjectId)
        {
            return await ExecuteTxAsync(txParams, gasObjectId);
        }

        public async Task<RpcResult<SuiExecuteTransactionResponse>> EnableSalesAsync(EnableSales txParams, string gasObjectId = null)
        {
            return await ExecuteTxAsync(txParams, gasObjectId);
        }

        public async Task<RpcResult<SuiExecuteTransactionResponse>> BuyNftCertificateAsync(BuyNftCertificate txParams, string gasObjectId = null)
        {
            return await ExecuteTxAsync(txParams, gasObjectId);
        }

        public async Task<RpcResult<SuiExecuteTransactionResponse>> CaimNftCertificateAsync(ClaimNftCertificate txParams, string gasObjectId = null)
        {
            return await ExecuteTxAsync(txParams, gasObjectId);
        }

        private async Task<RpcResult<SuiExecuteTransactionResponse>> ExecuteTxAsync(IMoveCallTransactionBuilder txBuilder, string gasObjectId = null)
        {
            var gas = gasObjectId ?? (await SuiHelper.GetCoinObjectIdsAboveBalancesOwnedByAddressAsync(_jsonRpcApiClient, txBuilder.Signer))[0];

            return await _signer.SignAndExecuteMoveCallAsync(txBuilder.BuildMoveCallTransaction(gas));
        }

    }
}
