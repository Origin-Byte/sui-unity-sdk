using Suinet.NftProtocol.Collection;
using Suinet.NftProtocol.Launchpad.Market;
using Suinet.NftProtocol.TransactionBuilders;
using Suinet.Rpc;
using Suinet.Rpc.Client;
using Suinet.Rpc.Signer;
using Suinet.Rpc.Types;
using System.Collections.Generic;
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

        public async Task<RpcResult<IEnumerable<T>>> GetNftsOwnedByAddressAsync<T>(string address) where T : class
        {
            var objects = await _jsonRpcApiClient.GetObjectsOwnedByAddressAsync<T>(address);
            return objects;
        }

        public async Task<RpcResult<IEnumerable<StdCollection>>> GetCollectionsOwnedByAddressAsync(string address)
        {
            var objects = await _jsonRpcApiClient.GetObjectsOwnedByAddressAsync<StdCollection>(address);
            return objects;
        }

        public async Task<RpcResult<SuiExecuteTransactionResponse>> MintNftAsync(MintNftToLaunchpad txParams, string gasObjectId)
        {
            return await ExecuteTxAsync(txParams, gasObjectId);
        }

        public async Task<RpcResult<IEnumerable<T>>> GetNftsAsync<T>(string[] objectIds) where T : class
        {
            return null;
           // var objects = await _jsonRpcApiClient.getobject
        }

        public async Task<RpcResult<IEnumerable<StdCollection>>> GetCollectionsAsync(string[] objectIds)
        {
            throw new System.NotImplementedException();
        }

        public async Task<RpcResult<IEnumerable<FixedPriceMarket>>> GetFixedPriceMarketsAsync(string[] objectIds)
        {
            throw new System.NotImplementedException();
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
