using Suinet.Rpc.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Suinet.Rpc.Api
{
    public interface IReadApi
    {
        /// <summary>
        /// Return the list of objects owned by an address.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        Task<RpcResult<IEnumerable<SuiObjectInfo>>> GetObjectsOwnedByAddressAsync(string address);

        /// <summary>
        /// Return the list of objects owned by an object.
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        Task<RpcResult<IEnumerable<SuiObjectInfo>>> GetObjectsOwnedByObjectAsync(string objectId);

        /// <summary>
        /// Return the total number of transactions known to the server.
        /// </summary>
        /// <returns></returns>
        Task<RpcResult<ulong>> GetTotalTransactionNumberAsync();

        /// <summary>
        /// Return list of transaction digests within the queried range.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        Task<RpcResult<IEnumerable<(ulong, string)>>> GetTransactionsInRangeAsync(ulong start, ulong end);

        /// <summary>
        /// Return list of recent transaction digest.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<RpcResult<IEnumerable<(ulong, string)>>> GetRecentTransactionsAsync(ulong count);

        /// <summary>
        /// Return the transaction response object.
        /// </summary>
        /// <param name="digest"></param>
        /// <returns></returns>
        Task<RpcResult<SuiTransactionResponse>> GetTransactionAsync(string digest);

        /// <summary>
        /// Return the object information for a specified object
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        Task<RpcResult<SuiObjectRead>> GetObjectAsync(string objectId);

        /// <summary>
        /// Return the list of dynamic field objects owned by an object.
        /// </summary>
        /// <param name="objectId">The ID of the parent object</param>
        /// <returns></returns>
        Task<RpcResult<SuiPage_for_DynamicFieldInfo_and_ObjectID>> GetDynamicFieldsAsync(string objectId);


        /// <summary>
        /// Return the dynamic field object information for a specified object
        /// </summary>
        /// <param name="parentObjectId"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        Task<RpcResult<SuiObjectRead>> GetDynamicFieldObjectAsync(string parentObjectId, string fieldName);
    }
}
