using Suinet.Rpc.Types;
using System.Threading.Tasks;

namespace Suinet.Rpc.Api
{
    public interface IExtendedApi
    {
        /// <summary>
        /// Return list of events for a specified query criteria.
        /// </summary>
        /// <param name="query">the event query criteria</param>
        /// <param name="cursor">optional paging cursor</param>
        /// <param name="limit">maximum number of items per page</param>
        /// <param name="query result ordering"></param>
        /// <returns></returns>
        Task<RpcResult<Page_for_Event_and_EventID>> QueryEventsAsync(EventFilter query, EventId cursor, ulong? limit, bool descendingOrder = false);

        /// <summary>
        /// Return the list of dynamic field objects owned by an object.
        /// </summary>
        /// <param name="parentObjectId">The ID of the parent object</param>
        /// <param name="cursor">An optional paging cursor. If provided, the query will start from the next item after the specified cursor. Default to start from the first item if not specified.</param>
        /// <param name="limit">Maximum item returned per page, default to [QUERY_MAX_RESULT_LIMIT] if not specified.</param>
        /// <returns></returns>
        Task<RpcResult<Page_for_DynamicFieldInfo_and_ObjectID>> GetDynamicFieldsAsync(string parentObjectId, string cursor, ulong? limit);


        /// <summary>
        /// Return the dynamic field object information for a specified object
        /// </summary>
        /// <param name="parentObjectId"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        Task<RpcResult<SuiObjectResponse>> GetDynamicFieldObjectAsync(string parentObjectId, DynamicFieldName fieldName);

        /// <summary>
        /// Return the list of objects owned by an address. Note that if the address owns more than `QUERY_MAX_RESULT_LIMIT` objects, the pagination is not accurate, because previous page may have been updated when the next page is fetched. Please use suix_queryObjects if this is a concern.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="query"></param>
        /// <param name="cursor"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<RpcResult<Page_for_SuiObjectResponse_and_ObjectID>> GetOwnedObjectsAsync(string address, ObjectResponseQuery query, string cursor, ulong? limit);

        /// <summary>
        /// Return list of transactions for a specified query criteria
        /// </summary>
        /// <param name="query">The transaction query criteria</param>
        /// <param name="cursor">An optional paging cursor. If provided, the query will start from the next item after the specified cursor. Default to start from the first item if not specified.</param>
        /// <param name="limit">Maximum item returned per page, default to QUERY_MAX_RESULT_LIMIT if not specified.</param>
        /// <param name="descendingOrder">Query result ordering, default to false (ascending order), oldest record first.</param>
        /// <returns></returns>
        Task<RpcResult<Page_for_TransactionBlockResponse_and_TransactionDigest>> QueryTransactionBlocksAsync(TransactionBlockResponseQuery query, EventId cursor, ulong? limit, bool? descendingOrder = false);

        /// <summary>
        /// Return the resolved address given resolver and name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<RpcResult<string>> ResolveNameServiceAddressAsync(string name);
    }
}
