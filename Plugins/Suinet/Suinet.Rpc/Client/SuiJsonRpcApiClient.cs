using Chaos.NaCl;
using Org.BouncyCastle.Crypto;
using Suinet.Rpc.Api;
using Suinet.Rpc.Client;
using Suinet.Rpc.Http;
using Suinet.Rpc.JsonRpc;
using Suinet.Rpc.Types;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Suinet.Rpc
{
    public partial class SuiJsonRpcApiClient : IReadApi, ITransactionBuilderApi, IQuorumDriverApi, IEventReadApi
    {
        private readonly IRpcClient _rpcClient;

        public SuiJsonRpcApiClient(IRpcClient rpcClient)
        {
            _rpcClient = rpcClient;
        }

        private JsonRpcRequest BuildRequest<T>(string method, IEnumerable<object> @params)
        {
            return new JsonRpcRequest(method, @params);
        }

        private async Task<RpcResult<T>> SendRpcRequestAsync<T>(string method)
        {
            var request = BuildRequest<T>(method, null);
            return await _rpcClient.SendAsync<T>(request);
        }

        private async Task<RpcResult<T>> SendRpcRequestAsync<T>(string method, IEnumerable<object> @params)
        {
            var request = BuildRequest<T>(method, @params);
            return await _rpcClient.SendAsync<T>(request);
        }

        public async Task<RpcResult<SuiTransactionBytes>> BatchTransactionAsync(string signer, IEnumerable<object> singleTransactionParams, string gas, ulong gasBudget)
        {
            return await SendRpcRequestAsync<SuiTransactionBytes>("sui_batchTransaction", ArgumentBuilder.BuildArguments(signer, singleTransactionParams, gas, gasBudget));
        }

        public async Task<RpcResult<SuiExecuteTransactionResponse>> ExecuteTransactionAsync(string txBytes, SuiSignatureScheme sigScheme, string signature, string pubKey, SuiExecuteTransactionRequestType suiExecuteTransactionRequestType)
        {
            // Todo refact this logic from here
            var signatureBytes = CryptoBytes.FromBase64String(signature);
            var publicKeyBytes = CryptoBytes.FromBase64String(pubKey);
            var finalSignatureBytes = new byte[signatureBytes.Length + 1 + publicKeyBytes.Length];

            finalSignatureBytes[0] = SignatureSchemeToByte(sigScheme);
            Array.Copy(signatureBytes, 0, finalSignatureBytes, 1, signatureBytes.Length);
            Array.Copy(publicKeyBytes, 0, finalSignatureBytes, signatureBytes.Length + 1, publicKeyBytes.Length);

            var serializedSignature = CryptoBytes.ToBase64String(finalSignatureBytes);

            return await SendRpcRequestAsync<SuiExecuteTransactionResponse>("sui_executeTransaction", ArgumentBuilder.BuildArguments(txBytes, serializedSignature, suiExecuteTransactionRequestType));
        }

        byte SignatureSchemeToByte(SuiSignatureScheme suiSignatureScheme)
        {
            if (suiSignatureScheme == SuiSignatureScheme.ED25519) return 0;

            return 1;
        }

        public async Task<RpcResult<SuiObjectRead>> GetObjectAsync(string objectId)
        {
            return await SendRpcRequestAsync<SuiObjectRead>("sui_getObject", ArgumentBuilder.BuildArguments(objectId));
        }

        public async Task<RpcResult<SuiObjectRead>> GetDynamicFieldObjectAsync(string parentObjectId, string fieldName)
        {
            return await SendRpcRequestAsync<SuiObjectRead>("sui_getDynamicFieldObject", ArgumentBuilder.BuildArguments(parentObjectId, fieldName));
        }

        public async Task<RpcResult<IEnumerable<SuiObjectInfo>>> GetObjectsOwnedByAddressAsync(string address)
        {
            return await SendRpcRequestAsync<IEnumerable<SuiObjectInfo>>("sui_getObjectsOwnedByAddress", ArgumentBuilder.BuildArguments(address));
        }

        public async Task<RpcResult<IEnumerable<SuiObjectInfo>>> GetObjectsOwnedByObjectAsync(string objectId)
        {
            return await SendRpcRequestAsync<IEnumerable<SuiObjectInfo>>("sui_getObjectsOwnedByObject", ArgumentBuilder.BuildArguments(objectId));
        }

        public async Task<RpcResult<IEnumerable<(ulong, string)>>> GetRecentTransactionsAsync(ulong count)
        {
            return await SendRpcRequestAsync<IEnumerable<(ulong, string)>>("sui_getRecentTransactions", ArgumentBuilder.BuildArguments(count));
        }

        public async Task<RpcResult<ulong>> GetTotalTransactionNumberAsync()
        {
            return await SendRpcRequestAsync<ulong>("sui_getTotalTransactionNumber");
        }

        public async Task<RpcResult<SuiTransactionResponse>> GetTransactionAsync(string digest)
        {
            return await SendRpcRequestAsync<SuiTransactionResponse>("sui_getTransaction", ArgumentBuilder.BuildArguments(digest));
        }

        public async Task<RpcResult<IEnumerable<(ulong, string)>>> GetTransactionsInRangeAsync(ulong start, ulong end)
        {
            return await SendRpcRequestAsync<IEnumerable<(ulong, string)>>("sui_getTransactionsInRange", ArgumentBuilder.BuildArguments(start, end));
        }

        public async Task<RpcResult<SuiTransactionBytes>> MoveCallAsync(string signer, string packageObjectId, string module, string function, IEnumerable<string> typeArguments, IEnumerable<object> arguments, string gas, ulong gasBudget)
        {
            return await SendRpcRequestAsync<SuiTransactionBytes>("sui_moveCall", ArgumentBuilder.BuildArguments(signer, packageObjectId, module, function, typeArguments, SuiJsonSanitizer.SanitizeArguments(arguments), gas, gasBudget));
        }

        public async Task<RpcResult<SuiTransactionBytes>> MoveCallAsync(MoveCallTransaction transactionParams)
        {
            return await MoveCallAsync(transactionParams.Signer, transactionParams.PackageObjectId, transactionParams.Module, transactionParams.Function, transactionParams.TypeArguments, transactionParams.Arguments, transactionParams.Gas, transactionParams.GasBudget);
        }

        public async Task<RpcResult<SuiTransactionBytes>> TransferObjectAsync(string signer, string objectId, string gas, ulong gasBudget, string recipient)
        {
            return await SendRpcRequestAsync<SuiTransactionBytes>("sui_transferObject", ArgumentBuilder.BuildArguments(signer, objectId, gas, gasBudget, recipient));
        }

        public async Task<RpcResult<SuiTransactionBytes>> TransferSuiAsync(string signer, string suiObjectId, ulong gasBudget, string recipient, ulong amount)
        {
            return await SendRpcRequestAsync<SuiTransactionBytes>("sui_transferSui", ArgumentBuilder.BuildArguments(signer, suiObjectId, gasBudget, recipient, amount));
        }

        public async Task<RpcResult<SuiTransactionBytes>> MergeCoinsAsync(string signer, string primaryCoinId, string coinToMergeId, string gas, ulong gasBudget)
        {
            return await SendRpcRequestAsync<SuiTransactionBytes>("sui_mergeCoins", ArgumentBuilder.BuildArguments(signer, primaryCoinId, coinToMergeId, gas, gasBudget));
        }

        public async Task<RpcResult<SuiTransactionBytes>> SplitCoinAsync(string signer, string coinObjectId, IEnumerable<ulong> splitAmounts, string gas, ulong gasBudget)
        {
            return await SendRpcRequestAsync<SuiTransactionBytes>("sui_splitCoin", ArgumentBuilder.BuildArguments(signer, coinObjectId, splitAmounts, gas, gasBudget));
        }

        public async Task<RpcResult<SuiTransactionBytes>> SplitCoinEqualAsync(string signer, string coinObjectId, ulong splitCount, string gas, ulong gasBudget)
        {
            return await SendRpcRequestAsync<SuiTransactionBytes>("sui_splitCoinEqual", ArgumentBuilder.BuildArguments(signer, coinObjectId, splitCount, gas, gasBudget));
        }

        public async Task<RpcResult<SuiTransactionBytes>> PayAsync(string signer, IEnumerable<string> inputCoins, IEnumerable<string> recipients, IEnumerable<ulong> amounts, string gas, ulong gasBudget)
        {
            return await SendRpcRequestAsync<SuiTransactionBytes>("sui_pay", ArgumentBuilder.BuildArguments(signer, inputCoins, recipients, amounts, gas, gasBudget));
        }

        public async Task<RpcResult<SuiPage_for_EventEnvelope_and_EventID>> GetEventsAsync(ISuiEventQuery query, SuiEventId cursor, ulong limit, bool descendingOrder = false)
        {
            return await SendRpcRequestAsync<SuiPage_for_EventEnvelope_and_EventID>("sui_getEvents", ArgumentBuilder.BuildArguments(query, cursor, limit, descendingOrder));
        }

        public async Task<RpcResult<SuiPage_for_DynamicFieldInfo_and_ObjectID>> GetDynamicFieldsAsync(string objectId)
        {
            return await SendRpcRequestAsync<SuiPage_for_DynamicFieldInfo_and_ObjectID>("sui_getDynamicFields", ArgumentBuilder.BuildArguments(objectId));
        }
    }

    public partial class SuiJsonRpcApiClient : IJsonRpcApiClient
    { }
}
