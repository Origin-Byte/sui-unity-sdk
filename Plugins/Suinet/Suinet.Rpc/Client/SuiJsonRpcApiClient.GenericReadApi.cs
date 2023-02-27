using Suinet.Rpc.Api;
using Suinet.Rpc.Types;
using Suinet.Rpc.Types.MoveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Suinet.Rpc
{
    /// <summary>
    /// Generic methods handling type conversions
    /// </summary>
    public partial class SuiJsonRpcApiClient : IGenericReadApi
    {
        public async Task<RpcResult<T>> GetObjectAsync<T>(string objectId) where T : class
        {
            var result = await GetObjectAsync(objectId);

            return new RpcResult<T>
            {
                IsSuccess = result.IsSuccess,
                RawRpcRequest = result.RawRpcRequest,
                RawRpcResponse = result.RawRpcResponse,
                ErrorMessage = result.ErrorMessage,
                Result = result.IsSuccess ? result.Result.Object.Data.Fields.ToObject<T>() : null
            };
        }

        public async Task<RpcResult<IEnumerable<T>>> GetObjectsOwnedByAddressAsync<T>(string address) where T : class
        {
            var rpcresult = await GetObjectsOwnedByAddressAsync(address);
            var objectsOwnedByAddress = new RpcResult<IEnumerable<T>>
            {
                IsSuccess = rpcresult.IsSuccess,
                RawRpcRequest = rpcresult.RawRpcRequest,
                RawRpcResponse = rpcresult.RawRpcResponse,
                ErrorMessage = rpcresult.ErrorMessage,
            };

            if (rpcresult.IsSuccess)
            {
                objectsOwnedByAddress.Result = await GetObjectsAsync<T>(rpcresult.Result);
            }

            return objectsOwnedByAddress;
        }

        public async Task<RpcResult<IEnumerable<T>>> GetObjectsOwnedByObjectAsync<T>(string objectId) where T : class
        {
            var rpcresult = await GetObjectsOwnedByObjectAsync(objectId);
            var objectsOwnedByAddress = new RpcResult<IEnumerable<T>>
            {
                IsSuccess = rpcresult.IsSuccess,
                RawRpcRequest = rpcresult.RawRpcRequest,
                RawRpcResponse = rpcresult.RawRpcResponse,
                ErrorMessage = rpcresult.ErrorMessage,
            };

            if (rpcresult.IsSuccess)
            {
                objectsOwnedByAddress.Result = await GetObjectsAsync<T>(rpcresult.Result);

            }

            return objectsOwnedByAddress;
        }


        public async Task<IEnumerable<T>> GetObjectsAsync<T>(IEnumerable<SuiObjectInfo> objectInfos) where T : class
        {
            var objectType = typeof(T);
            objectInfos = objectInfos.Where(x => MoveTypeHelper.IsMatchingMoveType(objectType, x.Type.Type)).ToArray();

            return await GetObjectsAsync<T>(objectInfos.Select(o => o.ObjectId));
        }

        public async Task<IEnumerable<T>> GetObjectsAsync<T>(IEnumerable<string> objectIds) where T : class
        {
            var typedObjects = new List<T>(objectIds.Count());
            foreach (var objectId in objectIds)
            {
                var obj = await GetObjectAsync<T>(objectId);

                if (obj.IsSuccess)
                {
                    typedObjects.Add(obj.Result);
                }
            }

            return typedObjects;
        }

        public async Task<RpcResult<T>> GetDynamicFieldObjectAsync<T>(string parentObjectId, string fieldName) where T : class
        {
            var result = await GetDynamicFieldObjectAsync(parentObjectId, fieldName);

            var dynamicFieldResult = result.Result.Object.Data.Fields.ToObject<DynamicField>();

            return new RpcResult<T>
            {
                IsSuccess = result.IsSuccess,
                RawRpcRequest = result.RawRpcRequest,
                RawRpcResponse = result.RawRpcResponse,
                ErrorMessage = result.ErrorMessage,
                Result = result.IsSuccess ? dynamicFieldResult.Value.Fields.ToObject<T>() : null
            };
        }

    }
}
