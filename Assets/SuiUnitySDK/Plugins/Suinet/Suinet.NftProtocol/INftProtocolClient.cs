using Suinet.NftProtocol.TransactionBuilders;
using Suinet.Rpc.Types;
using Suinet.Rpc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Suinet.NftProtocol.Collection;
using Suinet.NftProtocol.Launchpad.Market;

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

        Task<RpcResult<IEnumerable<T>>> GetNftsOwnedByAddressAsync<T>(string address) where T : class;

        Task<RpcResult<IEnumerable<StdCollection>>> GetCollectionsOwnedByAddressAsync(string address);

        Task<RpcResult<IEnumerable<T>>> GetNftsAsync<T>(string[] objectIds) where T : class;

        Task<RpcResult<IEnumerable<StdCollection>>> GetCollectionsAsync(string[] objectIds);

        Task<RpcResult<IEnumerable<FixedPriceMarket>>> GetFixedPriceMarketsAsync(string[] objectIds);
    }
}
