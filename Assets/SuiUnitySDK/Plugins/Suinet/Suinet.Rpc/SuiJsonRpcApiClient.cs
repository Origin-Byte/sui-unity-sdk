using Suinet.Rpc.Http;
using Suinet.Rpc.JsonRpc;
using Suinet.Rpc.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Suinet.Rpc
{
    public class SuiJsonRpcApiClient : IJsonRpcApiClient
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

        private IEnumerable<object> BuildParams(params object[] @params)
        {
            return @params;
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

        public async Task<RpcResult<SuiTransactionBytes>> BatchTransactionAsync(string signer, object[] singleTransactionParams, string gas, ulong gasBudget)
        {
            return await SendRpcRequestAsync<SuiTransactionBytes>("sui_batchTransaction", BuildParams(signer, singleTransactionParams, gas, gasBudget));
        }

        public async Task<RpcResult<SuiTransactionResponse>> ExecuteTransactionAsync(string txBytes, SuiSignatureScheme sigScheme, string signature, string pubKey)
        {
            return await SendRpcRequestAsync<SuiTransactionResponse>("sui_executeTransaction", BuildParams(txBytes, sigScheme, signature, pubKey, "ImmediateReturn"));
        }

        public async Task<RpcResult<SuiObjectRead>> GetObjectAsync(string objectId)
        {
            return await SendRpcRequestAsync<SuiObjectRead>("sui_getObject", BuildParams(objectId));
        }

        public async Task<RpcResult<IEnumerable<SuiObjectInfo>>> GetObjectsOwnedByAddressAsync(string address)
        {
            return await SendRpcRequestAsync<IEnumerable<SuiObjectInfo>>("sui_getObjectsOwnedByAddress", BuildParams(address));
        }

        public async Task<RpcResult<IEnumerable<SuiObjectInfo>>> GetObjectsOwnedByObjectAsync(string objectId)
        {
            return await SendRpcRequestAsync<IEnumerable<SuiObjectInfo>>("sui_getObjectsOwnedByObject", BuildParams(objectId));
        }

        public async Task<RpcResult<IEnumerable<(ulong, string)>>> GetRecentTransactionsAsync(ulong count)
        {
            return await SendRpcRequestAsync<IEnumerable<(ulong, string)>>("sui_getRecentTransactions", BuildParams(count));
        }

        public async Task<RpcResult<ulong>> GetTotalTransactionNumberAsync()
        {
            return await SendRpcRequestAsync<ulong>("sui_getTotalTransactionNumber");
        }

        public async Task<RpcResult<SuiTransactionResponse>> GetTransactionAsync(string digest)
        {
            return await SendRpcRequestAsync<SuiTransactionResponse>("sui_getTransaction", BuildParams(digest));
        }

        public async Task<RpcResult<IEnumerable<(ulong, string)>>> GetTransactionsInRangeAsync(ulong start, ulong end)
        {
            return await SendRpcRequestAsync<IEnumerable<(ulong, string)>>("sui_getTransactionsInRange", BuildParams(start, end));
        }

        public async Task<RpcResult<SuiTransactionBytes>> MoveCallAsync(string signer, string packageObjectId, string module, string function, IEnumerable<string> typeArguments, IEnumerable<object> arguments, string gas, ulong gasBudget)
        {
            return await SendRpcRequestAsync<SuiTransactionBytes>("sui_moveCall", BuildParams(signer, packageObjectId, module, function, typeArguments, arguments, gas, gasBudget));
        }

        public async Task<RpcResult<SuiTransactionBytes>> TransferObjectAsync(string signer, string objectId, string gas, ulong gasBudget, string recipient)
        {
            return await SendRpcRequestAsync<SuiTransactionBytes>("sui_transferObject", BuildParams(signer, objectId, gas, gasBudget, recipient));
        }

        public async Task<RpcResult<SuiTransactionBytes>> TransferSuiAsync(string signer, string suiObjectId, ulong gasBudget, string recipient, ulong amount)
        {
            return await SendRpcRequestAsync<SuiTransactionBytes>("sui_transferSui", BuildParams(signer, suiObjectId, gasBudget, recipient, amount));
        }

        public async Task<RpcResult<IEnumerable<SuiEventEnvelope>>> GetEventsByModuleAsync(string packageId, string moduleName, uint count, ulong startTime, ulong endTime)
        {
            return await SendRpcRequestAsync<IEnumerable<SuiEventEnvelope>>("sui_getEventsByModule", BuildParams(packageId, moduleName, count, startTime, endTime));
        }

        public async Task<RpcResult<IEnumerable<SuiEventEnvelope>>> GetEventsByMoveEventStructNameAsync(string moveEventStructName, uint count, ulong startTime, ulong endTime)
        {
            return await SendRpcRequestAsync<IEnumerable<SuiEventEnvelope>>("sui_getEventsByMoveEventStructName", BuildParams(moveEventStructName, count, startTime, endTime));
        }

        public async Task<RpcResult<IEnumerable<SuiEventEnvelope>>> GetEventsByObjectAsync(string objectId, uint count, ulong startTime, ulong endTime)
        {
            return await SendRpcRequestAsync<IEnumerable<SuiEventEnvelope>>("sui_getEventsByObject", BuildParams(objectId, count, startTime, endTime));
        }

        public async Task<RpcResult<IEnumerable<SuiEventEnvelope>>> GetEventsByRecipientAsync(object owner, uint count, ulong startTime, ulong endTime)
        {
            return await SendRpcRequestAsync<IEnumerable<SuiEventEnvelope>>("sui_getEventsByRecipient", BuildParams(owner, count, startTime, endTime));
        }

        public async Task<RpcResult<IEnumerable<SuiEventEnvelope>>> GetEventsBySenderAsync(string sender, uint count, ulong startTime, ulong endTime)
        {
            return await SendRpcRequestAsync<IEnumerable<SuiEventEnvelope>>("sui_getEventsBySender", BuildParams(sender, count, startTime, endTime));
        }

        public async Task<RpcResult<IEnumerable<SuiEventEnvelope>>> GetEventsByTimeRangeAsync(uint count, ulong startTime, ulong endTime)
        {
            return await SendRpcRequestAsync<IEnumerable<SuiEventEnvelope>>("sui_getEventsByTimeRange", BuildParams(count, startTime, endTime));
        }

        public async Task<RpcResult<IEnumerable<SuiEventEnvelope>>> GetEventsByTransactionAsync(string digest, uint count)
        {
            return await SendRpcRequestAsync<IEnumerable<SuiEventEnvelope>>("sui_getEventsByTransaction", BuildParams(digest, count));
        }
    }
}
