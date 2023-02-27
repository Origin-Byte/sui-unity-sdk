using Suinet.Rpc.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Suinet.Rpc.Api
{
    public interface IGenericReadApi
    {
        Task<RpcResult<T>> GetObjectAsync<T>(string objectId) where T : class;

        Task<RpcResult<T>> GetDynamicFieldObjectAsync<T>(string parentObjectId, string fieldName) where T : class;

        Task<RpcResult<IEnumerable<T>>> GetObjectsOwnedByAddressAsync<T>(string address) where T : class;

        Task<RpcResult<IEnumerable<T>>> GetObjectsOwnedByObjectAsync<T>(string objectId) where T : class;

        Task<IEnumerable<T>> GetObjectsAsync<T>(IEnumerable<SuiObjectInfo> objectInfos) where T : class;

        Task<IEnumerable<T>> GetObjectsAsync<T>(IEnumerable<string> objectIds) where T : class;
    }
}
