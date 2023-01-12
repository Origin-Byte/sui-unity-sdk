using Suinet.NftProtocol.TransactionBuilders;
using Suinet.Rpc.Types;
using Suinet.Rpc;
using System.Threading.Tasks;
using Suinet.NftProtocol.Nft;
using System.Collections.Generic;

namespace Suinet.NftProtocol
{
    public interface INftProtocolClient
    {
        Task<RpcResult<SuiExecuteTransactionResponse>> MintNftAsync(MintNft txParams, string gasObjectId = null);

        Task<RpcResult<SuiExecuteTransactionResponse>> EnableSalesAsync(EnableSales txParams, string gasObjectId = null);

        Task<RpcResult<ArtNft>> GetArtNftAsync(string objectId);

        Task<RpcResult<IEnumerable<ArtNft>>> GetArtNftsOwnedByAddressAsync(string address);
    }
}
