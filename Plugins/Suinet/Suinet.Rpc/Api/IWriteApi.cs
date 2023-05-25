using Suinet.Rpc.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Suinet.Rpc.Api
{
    public interface IWriteApi
    {
        /// <summary>
        /// Runs the transaction in dev-inspect mode. Which allows for nearly any transaction (or Move call) with any arguments. Detailed results are provided, including both the transaction effects and any return values.
        /// </summary>
        /// <param name="senderAddress"></param>
        /// <param name="txBytes">BCS encoded TransactionKind(as opposed to TransactionData, which include gasBudget and gasPrice)</param>
        /// <param name="gasPrice">Gas is not charged, but gas usage is still calculated. Default to use reference gas price</param>
        /// <param name="epoch">The epoch to perform the call. Will be set from the system state object if not provided</param>
        /// <returns></returns>
        Task<RpcResult<SuiDevInspectResults>> DevInspectTransactionBlockAsync(string senderAddress, string txBytes, ulong? gasPrice, ulong? epoch);

        /// <summary>
        /// Return transaction execution effects including the gas cost summary, while the effects are not committed to the chain.
        /// </summary>
        /// <param name="txBytes"></param>
        /// <returns></returns>
        Task<RpcResult<SuiDryRunTransactionBlockResponse>> DryRunTransactionBlockAsync(string txBytes);

        /// <summary>
        /// Execute the transaction and wait for results if desired. Request types: 
        /// 1. WaitForEffectsCert: waits for TransactionEffectsCert and then return to client.     This mode is a proxy for transaction finality. 
        /// 2. WaitForLocalExecution: waits for TransactionEffectsCert and make sure the node     executed the transaction locally before returning the client. The local execution     makes sure this node is aware of this transaction when client fires subsequent queries.     However if the node fails to execute the transaction locally in a timely manner,     a bool type in the response is set to false to indicated the case. request_type is default to be `WaitForEffectsCert` unless options.show_events or options.show_effects is true"
        /// </summary>
        /// <param name="txBytes">BCS serialized transaction data bytes without its type tag, as base-64 encoded string.</param>
        /// <returns></returns>
        Task<RpcResult<TransactionBlockResponse>> ExecuteTransactionBlockAsync(string txBytes, IEnumerable<string> serializedSignatures, TransactionBlockResponseOptions options, ExecuteTransactionRequestType requestType);
    }
}
