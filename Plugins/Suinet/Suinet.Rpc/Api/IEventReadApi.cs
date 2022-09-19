using Suinet.Rpc.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Suinet.Rpc.Api
{
    public interface IEventReadApi
    {
        /// <summary>
        /// Return events emitted in a specified Move module
        /// </summary>
        /// <param name="packageId">the Move package ID</param>
        /// <param name="moduleName">the module name</param>
        /// <param name="count">maximum size of the result, capped to EVENT_QUERY_MAX_LIMIT</param>
        /// <param name="startTime">left endpoint of time interval, inclusive</param>
        /// <param name="endTime">right endpoint of time interval, exclusive</param>
        /// <returns></returns>
        Task<RpcResult<IEnumerable<SuiEventEnvelope>>> GetEventsByModuleAsync(string packageId, string moduleName, uint count, ulong startTime, ulong endTime);

        /// <summary>
        /// Return events with the given move event struct name
        /// </summary>
        /// <param name="moveEventStructName">the event struct name type, e.g. `0x2::devnet_nft::MintNFTEvent` or `0x2::SUI::test_foo<address, vector<u8>>` with type params</param>
        /// <param name="count">maximum size of the result, capped to EVENT_QUERY_MAX_LIMIT</param>
        /// <param name="startTime">left endpoint of time interval, inclusive</param>
        /// <param name="endTime">right endpoint of time interval, exclusive</param>
        /// <returns></returns>
        Task<RpcResult<IEnumerable<SuiEventEnvelope>>> GetEventsByMoveEventStructNameAsync(string moveEventStructName, uint count, ulong startTime, ulong endTime);

        /// <summary>
        /// Return events associated with the given object
        /// </summary>
        /// <param name="objectId">the object ID</param>
        /// <param name="count">maximum size of the result, capped to EVENT_QUERY_MAX_LIMIT</param>
        /// <param name="startTime">left endpoint of time interval, inclusive</param>
        /// <param name="endTime">right endpoint of time interval, exclusive</param>
        /// <returns></returns>
        Task<RpcResult<IEnumerable<SuiEventEnvelope>>> GetEventsByObjectAsync(string objectId, uint count, ulong startTime, ulong endTime);

        /// <summary>
        /// Return events associated with the given recipient
        /// </summary>
        /// <param name="owner">the recipient</param>
        /// <param name="count">maximum size of the result, capped to EVENT_QUERY_MAX_LIMIT</param>
        /// <param name="startTime">left endpoint of time interval, inclusive</param>
        /// <param name="endTime">right endpoint of time interval, exclusive</param>
        /// <returns></returns>
        Task<RpcResult<IEnumerable<SuiEventEnvelope>>> GetEventsByRecipientAsync(object owner, uint count, ulong startTime, ulong endTime);

        /// <summary>
        /// Return events associated with the given sender
        /// </summary>
        /// <param name="sender">the sender's Sui address</param>
        /// <param name="count">maximum size of the result, capped to EVENT_QUERY_MAX_LIMIT</param>
        /// <param name="startTime">left endpoint of time interval, inclusive</param>
        /// <param name="endTime">right endpoint of time interval, exclusive</param>
        /// <returns></returns>
        Task<RpcResult<IEnumerable<SuiEventEnvelope>>> GetEventsBySenderAsync(string sender, uint count, ulong startTime, ulong endTime);

        /// <summary>
        /// Return events emitted in [start_time, end_time) interval
        /// </summary>
        /// <param name="count">maximum size of the result, capped to EVENT_QUERY_MAX_LIMIT</param>
        /// <param name="startTime">left endpoint of time interval, inclusive</param>
        /// <param name="endTime">right endpoint of time interval, exclusive</param>
        /// <returns></returns>
        Task<RpcResult<IEnumerable<SuiEventEnvelope>>> GetEventsByTimeRangeAsync(uint count, ulong startTime, ulong endTime);

        /// <summary>
        /// Return events emitted by the given transaction
        /// </summary>
        /// <param name="digest">digest of the transaction, as base-64 encoded string</param>
        /// <param name="count">maximum size of the result, capped to EVENT_QUERY_MAX_LIMIT</param>
        /// <returns></returns>
        Task<RpcResult<IEnumerable<SuiEventEnvelope>>> GetEventsByTransactionAsync(string digest, uint count);
    }
}
