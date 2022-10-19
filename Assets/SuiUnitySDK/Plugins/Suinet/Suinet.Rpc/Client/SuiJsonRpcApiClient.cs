using Suinet.Rpc.Api;
using Suinet.Rpc.Client;
using Suinet.Rpc.Http;
using Suinet.Rpc.JsonRpc;
using Suinet.Rpc.Types;
using System.Collections.Generic;
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
            return await SendRpcRequestAsync<SuiTransactionBytes>("sui_batchTransaction", TransactionUtils.BuildArguments(signer, singleTransactionParams, gas, gasBudget));
        }

        public async Task<RpcResult<SuiExecuteTransactionResponse>> ExecuteTransactionAsync(string txBytes, SuiSignatureScheme sigScheme, string signature, string pubKey, SuiExecuteTransactionRequestType suiExecuteTransactionRequestType)
        {
            return await SendRpcRequestAsync<SuiExecuteTransactionResponse>("sui_executeTransaction", TransactionUtils.BuildArguments(txBytes, sigScheme, signature, pubKey, suiExecuteTransactionRequestType));
        }

        public async Task<RpcResult<SuiObjectRead>> GetObjectAsync(string objectId)
        {
            return await SendRpcRequestAsync<SuiObjectRead>("sui_getObject", TransactionUtils.BuildArguments(objectId));
        }

        public async Task<RpcResult<IEnumerable<SuiObjectInfo>>> GetObjectsOwnedByAddressAsync(string address)
        {
            return await SendRpcRequestAsync<IEnumerable<SuiObjectInfo>>("sui_getObjectsOwnedByAddress", TransactionUtils.BuildArguments(address));
        }

        public async Task<RpcResult<IEnumerable<SuiObjectInfo>>> GetObjectsOwnedByObjectAsync(string objectId)
        {
            return await SendRpcRequestAsync<IEnumerable<SuiObjectInfo>>("sui_getObjectsOwnedByObject", TransactionUtils.BuildArguments(objectId));
        }

        public async Task<RpcResult<IEnumerable<(ulong, string)>>> GetRecentTransactionsAsync(ulong count)
        {
            return await SendRpcRequestAsync<IEnumerable<(ulong, string)>>("sui_getRecentTransactions", TransactionUtils.BuildArguments(count));
        }

        public async Task<RpcResult<ulong>> GetTotalTransactionNumberAsync()
        {
            return await SendRpcRequestAsync<ulong>("sui_getTotalTransactionNumber");
        }

        public async Task<RpcResult<SuiTransactionResponse>> GetTransactionAsync(string digest)
        {
            return await SendRpcRequestAsync<SuiTransactionResponse>("sui_getTransaction", TransactionUtils.BuildArguments(digest));
        }

        public async Task<RpcResult<IEnumerable<(ulong, string)>>> GetTransactionsInRangeAsync(ulong start, ulong end)
        {
            return await SendRpcRequestAsync<IEnumerable<(ulong, string)>>("sui_getTransactionsInRange", TransactionUtils.BuildArguments(start, end));
        }

        public async Task<RpcResult<SuiTransactionBytes>> MoveCallAsync(string signer, string packageObjectId, string module, string function, IEnumerable<string> typeArguments, IEnumerable<object> arguments, string gas, ulong gasBudget)
        {
            return await SendRpcRequestAsync<SuiTransactionBytes>("sui_moveCall", TransactionUtils.BuildArguments(signer, packageObjectId, module, function, typeArguments, arguments, gas, gasBudget));
        }

        public async Task<RpcResult<SuiTransactionBytes>> MoveCallAsync(MoveCallTransaction transactionParams)
        {
            return await MoveCallAsync(transactionParams.Signer, transactionParams.PackageObjectId, transactionParams.Module, transactionParams.Function, transactionParams.TypeArguments, transactionParams.Arguments, transactionParams.Gas, transactionParams.GasBudget);
        }

        public async Task<RpcResult<SuiTransactionBytes>> TransferObjectAsync(string signer, string objectId, string gas, ulong gasBudget, string recipient)
        {
            return await SendRpcRequestAsync<SuiTransactionBytes>("sui_transferObject", TransactionUtils.BuildArguments(signer, objectId, gas, gasBudget, recipient));
        }

        public async Task<RpcResult<SuiTransactionBytes>> TransferSuiAsync(string signer, string suiObjectId, ulong gasBudget, string recipient, ulong amount)
        {
            return await SendRpcRequestAsync<SuiTransactionBytes>("sui_transferSui", TransactionUtils.BuildArguments(signer, suiObjectId, gasBudget, recipient, amount));
        }

        public async Task<RpcResult<IEnumerable<SuiEventEnvelope>>> GetEventsByModuleAsync(string packageId, string moduleName, uint count, ulong startTime, ulong endTime)
        {
            return await SendRpcRequestAsync<IEnumerable<SuiEventEnvelope>>("sui_getEventsByModule", TransactionUtils.BuildArguments(packageId, moduleName, count, startTime, endTime));
        }

        public async Task<RpcResult<IEnumerable<SuiEventEnvelope>>> GetEventsByMoveEventStructNameAsync(string moveEventStructName, uint count, ulong startTime, ulong endTime)
        {
            return await SendRpcRequestAsync<IEnumerable<SuiEventEnvelope>>("sui_getEventsByMoveEventStructName", TransactionUtils.BuildArguments(moveEventStructName, count, startTime, endTime));
        }

        public async Task<RpcResult<IEnumerable<SuiEventEnvelope>>> GetEventsByObjectAsync(string objectId, uint count, ulong startTime, ulong endTime)
        {
            return await SendRpcRequestAsync<IEnumerable<SuiEventEnvelope>>("sui_getEventsByObject", TransactionUtils.BuildArguments(objectId, count, startTime, endTime));
        }

        public async Task<RpcResult<IEnumerable<SuiEventEnvelope>>> GetEventsByRecipientAsync(object owner, uint count, ulong startTime, ulong endTime)
        {
            return await SendRpcRequestAsync<IEnumerable<SuiEventEnvelope>>("sui_getEventsByRecipient", TransactionUtils.BuildArguments(owner, count, startTime, endTime));
        }

        public async Task<RpcResult<IEnumerable<SuiEventEnvelope>>> GetEventsBySenderAsync(string sender, uint count, ulong startTime, ulong endTime)
        {
            return await SendRpcRequestAsync<IEnumerable<SuiEventEnvelope>>("sui_getEventsBySender", TransactionUtils.BuildArguments(sender, count, startTime, endTime));
        }

        public async Task<RpcResult<IEnumerable<SuiEventEnvelope>>> GetEventsByTimeRangeAsync(uint count, ulong startTime, ulong endTime)
        {
            return await SendRpcRequestAsync<IEnumerable<SuiEventEnvelope>>("sui_getEventsByTimeRange", TransactionUtils.BuildArguments(count, startTime, endTime));
        }

        public async Task<RpcResult<IEnumerable<SuiEventEnvelope>>> GetEventsByTransactionAsync(string digest, uint count)
        {
            return await SendRpcRequestAsync<IEnumerable<SuiEventEnvelope>>("sui_getEventsByTransaction", TransactionUtils.BuildArguments(digest, count));
        }

        public async Task<RpcResult<SuiTransactionBytes>> MergeCoinsAsync(string signer, string primaryCoinId, string coinToMergeId, string gas, ulong gasBudget)
        {
            return await SendRpcRequestAsync<SuiTransactionBytes>("sui_mergeCoins", TransactionUtils.BuildArguments(signer, primaryCoinId, coinToMergeId, gas, gasBudget));
        }

        public async Task<RpcResult<SuiTransactionBytes>> SplitCoinAsync(string signer, string coinObjectId, IEnumerable<ulong> splitAmounts, string gas, ulong gasBudget)
        {
            return await SendRpcRequestAsync<SuiTransactionBytes>("sui_splitCoin", TransactionUtils.BuildArguments(signer, coinObjectId, splitAmounts, gas, gasBudget));
        }

        public async Task<RpcResult<SuiTransactionBytes>> SplitCoinEqualAsync(string signer, string coinObjectId, ulong splitCount, string gas, ulong gasBudget)
        {
            return await SendRpcRequestAsync<SuiTransactionBytes>("sui_splitCoinEqual", TransactionUtils.BuildArguments(signer, coinObjectId, splitCount, gas, gasBudget));
        }

        public async Task<RpcResult<SuiTransactionBytes>> PayAsync(string signer, IEnumerable<string> inputCoins, IEnumerable<string> recipients, IEnumerable<ulong> amounts, string gas, ulong gasBudget)
        {
            return await SendRpcRequestAsync<SuiTransactionBytes>("sui_pay", TransactionUtils.BuildArguments(signer, inputCoins, recipients, amounts, gas, gasBudget));
        }
    }

    public partial class SuiJsonRpcApiClient : IJsonRpcApiClient
    { }
}
