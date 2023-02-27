using Suinet.NftProtocol.Domains;
using Suinet.NftProtocol.Nft;
using Suinet.NftProtocol.TransactionBuilders;
using Suinet.Rpc;
using Suinet.Rpc.Client;
using Suinet.Rpc.Signer;
using Suinet.Rpc.Types;
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

        public async Task<RpcResult<ArtNft>> GetArtNftAsync(string objectId, params Type[] withDomains)
        {
            var nftResult = await _jsonRpcApiClient.GetObjectAsync<ArtNft>(objectId);

            if (nftResult == null || !nftResult.IsSuccess) return nftResult;

            await LoadDomainsForArtNftAsync(nftResult.Result, withDomains);

            return nftResult;
        }


        public async Task<RpcResult<IEnumerable<ArtNft>>> GetArtNftsOwnedByAddressAsync(string address, params Type[] withDomains)
        {
            var nftsResult = await _jsonRpcApiClient.GetObjectsOwnedByAddressAsync<ArtNft>(address);

            if (nftsResult == null || !nftsResult.IsSuccess) return nftsResult;

            // TODO test is Task.WhenAll works correctly on webgl
            foreach (var nft in nftsResult.Result)
            {
                await LoadDomainsForArtNftAsync(nft, withDomains);
            }

            return nftsResult;
        }

        private async Task LoadDomainsForArtNftAsync(ArtNft nft, params Type[] withDomains)
        {
            var parentObjectId = nft.Id.Id;
            var dynamicFields = await _jsonRpcApiClient.GetDynamicFieldsAsync(parentObjectId);

            bool filterDomains = withDomains != null && withDomains.Any();

            if (!filterDomains || withDomains.Contains(typeof(UrlDomain)))
            {
                var objectFieldInfo = dynamicFields.Result.Data.FirstOrDefault(d => d.ObjectType.Struct == nameof(UrlDomain));

                if (objectFieldInfo != null) 
                {
                    var domainResult = await _jsonRpcApiClient.GetDynamicFieldObjectAsync<UrlDomain>(parentObjectId, objectFieldInfo.Name);
                    if (domainResult != null && domainResult.IsSuccess)
                    {
                        nft.Url = domainResult.Result.Url;
                    }
                }
            }

            if (!filterDomains || withDomains.Contains(typeof(DisplayDomain)))
            {
                var objectFieldInfo = dynamicFields.Result.Data.FirstOrDefault(d => d.ObjectType.Struct == nameof(DisplayDomain));

                if (objectFieldInfo != null)
                {
                    var domainResult = await _jsonRpcApiClient.GetDynamicFieldObjectAsync<DisplayDomain>(parentObjectId, objectFieldInfo.Name);
                    if (domainResult != null && domainResult.IsSuccess)
                    {
                        var domain = domainResult.Result;
                        nft.Name = domain?.Name;
                        nft.Description = domain?.Description;
                    }
                }
            }

            if (!filterDomains || withDomains.Contains(typeof(AttributesDomain)))
            {
                var objectFieldInfo = dynamicFields.Result.Data.FirstOrDefault(d => d.ObjectType.Struct == nameof(AttributesDomain));

                if (objectFieldInfo != null)
                {
                    var domainResult = await _jsonRpcApiClient.GetDynamicFieldObjectAsync<AttributesDomain>(parentObjectId, objectFieldInfo.Name);

                    if (domainResult != null && domainResult.IsSuccess)
                    {
                        nft.Attributes = domainResult.Result.Attributes;
                    }
                }
            }
        }
    }
}
