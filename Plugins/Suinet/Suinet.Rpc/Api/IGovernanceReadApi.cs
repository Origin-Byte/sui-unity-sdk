using Suinet.Rpc.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Suinet.Rpc.Api
{
    public interface IGovernanceReadApi
    {
        /// <summary>
        /// Return the latest SUI system state object on-chain.
        /// </summary>
        /// <returns>SuiSystemStateSummary</returns>
        Task<RpcResult<SuiSystemStateSummary>> GetLatestSuiSystemStateAsync();

        /// <summary>
        /// Return the reference gas price for the network.
        /// </summary>
        /// <returns></returns>
        Task<RpcResult<ulong>> GetReferenceGasPriceAsync();

        /// <summary>
        /// Return all [DelegatedStake].
        /// </summary>
        /// <returns></returns>
        Task<RpcResult<SuiDelegatedStake>> GetStakesAsync(string address);

        /// <summary>
        /// Return one or more [DelegatedStake]. If a Stake was withdrawn its status will be Unstaked.
        /// </summary>
        /// <param name="objectIds"></param>
        /// <returns></returns>
        Task<RpcResult<SuiDelegatedStake[]>> GetStakesByIdsAsync(IEnumerable<string> objectIds);

        /// <summary>
        /// Return the validator APY
        /// </summary>
        /// <returns></returns>
        Task<RpcResult<ValidatorApys>> GetValidatorsApy();
    }
}
