using Suinet.NftProtocol.Domains;
using Suinet.NftProtocol.Examples;
using Suinet.NftProtocol.Nft;
using Suinet.NftProtocol.TransactionBuilders;
using Suinet.Rpc;
using Suinet.Rpc.Client;
using Suinet.Rpc.Types;
using Suinet.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Suinet.NftProtocol
{
    public class NftProtocolClient : INftProtocolClient
    {
        private readonly IJsonRpcApiClient _jsonRpcApiClient;
        private readonly IKeyPair _keypair;

        public NftProtocolClient(IJsonRpcApiClient jsonRpcApiClient, IKeyPair keypair)
        {
            _jsonRpcApiClient = jsonRpcApiClient;
            _keypair = keypair;
        }

        public async Task<RpcResult<TransactionBlockResponse>> MintNftAsync(MintSuitradersNft txParams, string gasObjectId)
        {
            return await ExecuteTxAsync(txParams, gasObjectId);
        }

        public async Task<RpcResult<TransactionBlockResponse>> EnableSalesAsync(EnableSales txParams, string gasObjectId = null)
        {
            return await ExecuteTxAsync(txParams, gasObjectId);
        }

        private async Task<RpcResult<TransactionBlockResponse>> ExecuteTxAsync(IMoveCallTransactionBuilder txBuilder, string gasObjectId = null)
        {
            var moveCallResult = await _jsonRpcApiClient.MoveCallAsync(txBuilder.BuildMoveCallTransaction(gasObjectId));

            var txBytes = moveCallResult.Result.TxBytes;
            var rawSigner = new RawSigner(_keypair);
            var signature = rawSigner.SignData(Intent.GetMessageWithIntent(txBytes));

            var txResponse = await _jsonRpcApiClient.ExecuteTransactionBlockAsync(txBytes, new[] { signature.Value }, TransactionBlockResponseOptions.ShowAll(), ExecuteTransactionRequestType.WaitForLocalExecution);

            return txResponse;
        }

        public async Task<RpcResult<ArtNft>> GetArtNftAsync(string objectId, params Type[] withDomains)
        {
            var nftResult = await _jsonRpcApiClient.GetObjectAsync<ArtNft>(objectId, new ArtNftParser());

            //if (nftResult == null || !nftResult.IsSuccess) return nftResult;

            //await LoadDomainsForArtNftAsync(nftResult.Result, withDomains);

            return nftResult;
        }


        public async Task<RpcResult<IEnumerable<ArtNft>>> GetArtNftsOwnedByAddressAsync(string address, params Type[] withDomains)
        {

            var filter = ObjectDataFilterFactory.CreateMatchAllFilter(ObjectDataFilterFactory.CreateAddressOwnerFilter(address));
            var query = new ObjectResponseQuery() {Filter = filter, Options = ObjectDataOptions.ShowAll()};
            return await _jsonRpcApiClient.GetOwnedObjectsAsync<ArtNft>(address, new ArtNftParser(), query
                , null, null);
        }

        private async Task LoadDomainsForArtNftAsync(ArtNft nft, params Type[] withDomains)
        {
            throw new NotImplementedException(); // TODO parsers
            var parentObjectId = nft.Id.Id;
            var dynamicFields = await _jsonRpcApiClient.GetDynamicFieldsAsync(parentObjectId, null, null);

            bool filterDomains = withDomains != null && withDomains.Any();

            if (!filterDomains || withDomains.Contains(typeof(UrlDomain)))
            {
                var objectFieldInfo = dynamicFields.Result.Data.FirstOrDefault(d => d.ObjectType.Struct == nameof(UrlDomain));

                if (objectFieldInfo != null) 
                {
                    var domainResult = await _jsonRpcApiClient.GetDynamicFieldObjectAsync<UrlDomain>(parentObjectId, objectFieldInfo.Name, null);
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
                    var domainResult = await _jsonRpcApiClient.GetDynamicFieldObjectAsync<DisplayDomain>(parentObjectId, objectFieldInfo.Name, null);
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
                    var domainResult = await _jsonRpcApiClient.GetDynamicFieldObjectAsync<AttributesDomain>(parentObjectId, objectFieldInfo.Name, null);

                    if (domainResult != null && domainResult.IsSuccess)
                    {
                        //nft.Attributes = domainResult.Result.Attributes;
                    }
                }
            }
        }

        public async Task<RpcResult<IEnumerable<ArtNft>>> GetArtNftsFromKioskAsync(string kioskId)
        {
            throw new NotImplementedException();
            //var kiosk = await 
        }
    }
}
