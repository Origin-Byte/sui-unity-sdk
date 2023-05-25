using Suinet.Rpc.Api;
using Suinet.Rpc.Types;
using Suinet.Rpc.Types.ObjectDataParsers;
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
        public async Task<RpcResult<T>> GetObjectAsync<T>(string objectId, IObjectDataParser<T> parser, ObjectDataOptions options) where T : class
        {
            options = options ?? ObjectDataOptions.ShowAll();
            var result = await GetObjectAsync(objectId, options);   

            var typedResult = new RpcResult<T>
            {
                IsSuccess = result.IsSuccess,
                RawRpcRequest = result.RawRpcRequest,
                RawRpcResponse = result.RawRpcResponse,
                ErrorMessage = result.ErrorMessage,
            };

            if (result.IsSuccess)
            {
                typedResult.Result = parser.TypeRegex.IsMatch(result.Result.Data.Type) ? parser.Parse(result.Result.Data) : null;
            }

            return typedResult;
        }

        public async Task<RpcResult<IEnumerable<T>>> GetObjectsOwnedByAddressAsync<T>(string address, IObjectDataParser<T> parser, string cursor, ulong? limit, ObjectDataOptions options) where T : class
        {
            var filter = ObjectDataFilterFactory.CreateMatchAllFilter(ObjectDataFilterFactory.CreateAddressOwnerFilter(address));
            options = options ?? ObjectDataOptions.ShowAll();
            var rpcresult = await GetOwnedObjectsAsync(address, new ObjectResponseQuery() { Filter = filter, Options = options }, cursor, limit);
            var objectsOwnedByAddress = new RpcResult<IEnumerable<T>>
            {
                IsSuccess = rpcresult.IsSuccess,
                RawRpcRequest = rpcresult.RawRpcRequest,
                RawRpcResponse = rpcresult.RawRpcResponse,
                ErrorMessage = rpcresult.ErrorMessage,
            };

            if (rpcresult.IsSuccess)
            {
                objectsOwnedByAddress.Result = rpcresult.Result.Data.Where(d => parser.TypeRegex.IsMatch(d.Data.Type)).Select(d => parser.Parse(d.Data));
            }

            return objectsOwnedByAddress;
        }

        public async Task<RpcResult<IEnumerable<T>>> GetOwnedObjectsAsync<T>(string address, IObjectDataParser<T> parser, ObjectResponseQuery query, string cursor, ulong? limit) where T : class
        {
            var rpcresult = await GetOwnedObjectsAsync(address, query, cursor, limit);

            var objectsOwned = new RpcResult<IEnumerable<T>>
            {
                IsSuccess = rpcresult.IsSuccess,
                RawRpcRequest = rpcresult.RawRpcRequest,
                RawRpcResponse = rpcresult.RawRpcResponse,
                ErrorMessage = rpcresult.ErrorMessage,
            };

            if (rpcresult.IsSuccess)
            {
                objectsOwned.Result = rpcresult.Result.Data.Where(d => parser.TypeRegex.IsMatch(d.Data.Type)).Select(d => parser.Parse(d.Data));
            }

            return objectsOwned;
        }

        public async Task<RpcResult<IEnumerable<T>>> GetObjectsAsync<T>(IEnumerable<string> objectIds, IObjectDataParser<T> parser, ObjectDataOptions options) where T : class
        {
            options = options ?? ObjectDataOptions.ShowAll();
            var rpcresult = await GetObjectsAsync(objectIds, options);

            var objectsOwned = new RpcResult<IEnumerable<T>>
            {
                IsSuccess = rpcresult.IsSuccess,
                RawRpcRequest = rpcresult.RawRpcRequest,
                RawRpcResponse = rpcresult.RawRpcResponse,
                ErrorMessage = rpcresult.ErrorMessage,
            };

            if (rpcresult.IsSuccess)
            {
                objectsOwned.Result = rpcresult.Result.Where(d => parser.TypeRegex.IsMatch(d.Data.Type)).Select(d => parser.Parse(d.Data));
            }

            return objectsOwned;

        }     

        public async Task<RpcResult<T>> GetDynamicFieldObjectAsync<T>(string parentObjectId, DynamicFieldName fieldName, IObjectDataParser<T> parser) where T : class
        {
            var result = await GetDynamicFieldObjectAsync(parentObjectId, fieldName);

            var dynamicFieldObject = new RpcResult<T>
            {
                IsSuccess = result.IsSuccess,
                RawRpcRequest = result.RawRpcRequest,
                RawRpcResponse = result.RawRpcResponse,
                ErrorMessage = result.ErrorMessage,
            };


            if (result.IsSuccess)
            {
                dynamicFieldObject.Result = parser.Parse(result.Result.Data);
            }

            return dynamicFieldObject;
        }

    }
}
