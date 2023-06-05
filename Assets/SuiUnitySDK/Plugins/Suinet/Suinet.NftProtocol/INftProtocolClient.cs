using Suinet.NftProtocol.TransactionBuilders;
using Suinet.Rpc.Types;
using Suinet.Rpc;
using System.Threading.Tasks;
using Suinet.NftProtocol.Nft;
using System.Collections.Generic;
using Suinet.NftProtocol.Domains;
using System;
using Suinet.NftProtocol.Examples;

namespace Suinet.NftProtocol
{
    public interface INftProtocolClient
    {
        Task<RpcResult<TransactionBlockResponse>> MintNftAsync(MintSuitradersNft txParams, string gasObjectId = null);

        Task<RpcResult<TransactionBlockResponse>> EnableSalesAsync(EnableSales txParams, string gasObjectId = null);

        /// <summary>
        /// Retrieves an Art nft. If withDomains types are not provided, queryies all domains
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="withDomains"></param>
        /// <returns></returns>
        Task<RpcResult<ArtNft>> GetArtNftAsync(string objectId, params Type[] withDomains);

        /// <summary>
        /// Retrieves all Art nfts for an address. If withDomains types are not provided, queryies all domains
        /// </summary>
        /// <param name="address"></param>
        /// <param name="withDomains"></param>
        /// <returns></returns>
        Task<RpcResult<IEnumerable<ArtNft>>> GetArtNftsOwnedByAddressAsync(string address, params Type[] withDomains);

        Task<RpcResult<IEnumerable<ArtNft>>> GetArtNftsFromKioskAsync(string kioskId);
    }
}
