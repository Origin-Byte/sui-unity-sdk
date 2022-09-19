using Suinet.Rpc.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Suinet.Rpc.Api
{
    public interface ITransactionBuilderApi
    {
        /// <summary>
        /// Create an unsigned transaction to transfer an object from one address to another. The object's type must allow public transfers
        /// </summary>
        /// <param name="signer"></param>
        /// <param name="objectId"></param>
        /// <param name="gas"></param>
        /// <param name="gasBudget"></param>
        /// <param name="recipient"></param>
        /// <returns></returns>
        Task<RpcResult<SuiTransactionBytes>> TransferObjectAsync(string signer, string objectId, string gas, ulong gasBudget, string recipient);

        /// <summary>
        /// Create an unsigned transaction to send SUI coin object to a Sui address. The SUI object is also used as the gas object.
        /// </summary>
        /// <param name="signer"></param>
        /// <param name="suiObjectId"></param>
        /// <param name="gasBudget"></param>
        /// <param name="recipient"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        Task<RpcResult<SuiTransactionBytes>> TransferSuiAsync(string signer, string suiObjectId, ulong gasBudget, string recipient, ulong amount);

        /// <summary>
        /// Create an unsigned transaction to execute a Move call on the network, by calling the specified function in the module of a given package.
        /// </summary>
        /// <param name="signer"></param>
        /// <param name="packageObjectId"></param>
        /// <param name="module"></param>
        /// <param name="function"></param>
        /// <param name="typeArguments"></param>
        /// <param name="arguments"></param>
        /// <param name="gas"></param>
        /// <param name="gasBudget"></param>
        /// <returns></returns>
        Task<RpcResult<SuiTransactionBytes>> MoveCallAsync(string signer, string packageObjectId, string module, string function, IEnumerable<string> typeArguments, IEnumerable<object> arguments, string gas, ulong gasBudget);

        /// <summary>
        /// Create an unsigned batched transaction.
        /// </summary>
        /// <param name="signer"></param>
        /// <param name="singleTransactionParams"></param>
        /// <param name="gas"></param>
        /// <param name="gasBudget"></param>
        /// <returns></returns>
        Task<RpcResult<SuiTransactionBytes>> BatchTransactionAsync(string signer, object[] singleTransactionParams, string gas, ulong gasBudget);
    }
}
