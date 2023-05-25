using Suinet.Rpc.Types;
using System.Threading.Tasks;

namespace Suinet.Rpc.Api
{
    public interface ICoinQueryApi
    {
        /// <summary>
        /// Return the total coin balance for all coin type, owned by the address owner.
        /// </summary>
        /// <param name="ownerAddress">the owner's Sui address</param>
        /// <returns></returns>
        Task<RpcResult<Balance[]>> GetAllBalancesAsync(string ownerAddress);

        /// <summary>
        /// Return all Coin objects owned by an address
        /// </summary>
        /// <param name="ownerAddress">the owner's Sui address</param>
        /// <param name="cursor">optional paging cursor</param>
        /// <param name="limit">maximum number of items per page</param>
        /// <returns></returns>
        Task<RpcResult<SuiPage_for_Coin_and_ObjectID>> GetAllCoinsAsync(string ownerAddress, string cursor, ulong limit);

        /// <summary>
        /// Return the total coin balance for one coin type, owned by the address owner.
        /// </summary>
        /// <param name="ownerAddress"></param>
        /// <param name="coinType"></param>
        /// <returns></returns>
        Task<RpcResult<Balance>> GetBalanceAsync(string ownerAddress, string coinType);
    }
}
