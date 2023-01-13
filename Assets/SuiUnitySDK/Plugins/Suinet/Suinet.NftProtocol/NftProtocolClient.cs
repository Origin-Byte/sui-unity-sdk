using Suinet.NftProtocol.Domains;
using Suinet.NftProtocol.Nft;
using Suinet.NftProtocol.TransactionBuilders;
using Suinet.Rpc;
using Suinet.Rpc.Client;
using Suinet.Rpc.Signer;
using Suinet.Rpc.Types;
using Suinet.Rpc.Types.MoveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<RpcResult<SuiExecuteTransactionResponse>> MintNftAsync(MintNft txParams, string gasObjectId)
        {
            return await ExecuteTxAsync(txParams, gasObjectId);
        }

        public async Task<RpcResult<SuiExecuteTransactionResponse>> EnableSalesAsync(EnableSales txParams, string gasObjectId = null)
        {
            return await ExecuteTxAsync(txParams, gasObjectId);
        }

        private async Task<RpcResult<SuiExecuteTransactionResponse>> ExecuteTxAsync(IMoveCallTransactionBuilder txBuilder, string gasObjectId = null)
        {
            var gas = gasObjectId ?? (await SuiHelper.GetCoinObjectIdsAboveBalancesOwnedByAddressAsync(_jsonRpcApiClient, txBuilder.Signer))[0];

            return await _signer.SignAndExecuteMoveCallAsync(txBuilder.BuildMoveCallTransaction(gas));
        }

        public async Task<RpcResult<ArtNft>> GetArtNftAsync(string objectId)
        {
            var nftResult = await _jsonRpcApiClient.GetObjectAsync<ArtNft>(objectId);

            if (nftResult == null || !nftResult.IsSuccess) return nftResult;

            await LoadDomainsForArtNftAsync(nftResult.Result);

            return nftResult;
        }


        public async Task<RpcResult<IEnumerable<ArtNft>>> GetArtNftsOwnedByAddressAsync(string address)
        {
            var nftsResult = await _jsonRpcApiClient.GetObjectsOwnedByAddressAsync<ArtNft>(address);

            if (nftsResult == null || !nftsResult.IsSuccess) return nftsResult;

            foreach (var nft in nftsResult.Result)
            {
                await LoadDomainsForArtNftAsync(nft);
            }

            return nftsResult;
        }

        private async Task LoadDomainsForArtNftAsync(ArtNft nft)
        {
            var bagObjectId = nft.Bag.Fields.Id.Id;

            var urlDomainResult = await _jsonRpcApiClient.GetObjectsOwnedByObjectAsync<UrlDomain>(bagObjectId);

            if (urlDomainResult != null && urlDomainResult.IsSuccess)
            {
                nft.Url = urlDomainResult.Result.FirstOrDefault()?.Url;
            }

            var displayDomainResult = await _jsonRpcApiClient.GetObjectsOwnedByObjectAsync<DisplayDomain>(bagObjectId);

            if (displayDomainResult != null && displayDomainResult.IsSuccess)
            {
                var domain = displayDomainResult.Result.FirstOrDefault();
                nft.Name = domain?.DisplayName;
                nft.Description = domain?.Description;
            }

            var attributesDomainResult = await _jsonRpcApiClient.GetObjectsOwnedByObjectAsync<AttributesDomain>(bagObjectId);

            if (attributesDomainResult != null && attributesDomainResult.IsSuccess)
            {
                nft.Attributes = attributesDomainResult.Result.FirstOrDefault()?.Attributes;
            }
        }
    }
}
