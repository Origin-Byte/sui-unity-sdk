using System.Numerics;

namespace Suinet.Rpc.Types
{
    public class SuiValidatorSummary
    {
        public BigInteger CommissionRate { get; set; }

        public string ExchangeRatesId { get; set; }

        public BigInteger ExchangeRatesSize { get; set; }

        public BigInteger GasPrice { get; set; }

        public string ImageUrl { get; set; }

        public string Name { get; set; }

        public string NetAddress { get; set; }

        public string NetworkPubkeyBytes { get; set; }

        public BigInteger NextEpochCommissionRate { get; set; }

        public BigInteger NextEpochGasPrice { get; set; }

        public string NextEpochNetAddress { get; set; }

        public string NextEpochNetworkPubkeyBytes { get; set; }

        public string NextEpochP2pAddress { get; set; }

        public string NextEpochPrimaryAddress { get; set; }

        public string NextEpochProofOfPossession { get; set; }

        public string NextEpochProtocolPubkeyBytes { get; set; }

        public BigInteger NextEpochStake { get; set; }

        public string NextEpochWorkerAddress { get; set; }

        public string NextEpochWorkerPubkeyBytes { get; set; }

        public string OperationCapId { get; set; }

        public string P2pAddress { get; set; }

        public BigInteger PendingPoolTokenWithdraw { get; set; }

        public BigInteger PendingStake { get; set; }

        public BigInteger PendingTotalSuiWithdraw { get; set; }

        public BigInteger PoolTokenBalance { get; set; }

        public string PrimaryAddress { get; set; }

        public string ProjectUrl { get; set; }

        public string ProofOfPossessionBytes { get; set; }

        public string ProtocolPubkeyBytes { get; set; }

        public BigInteger RewardsPool { get; set; }

        public BigInteger? StakingPoolActivationEpoch { get; set; }

        public BigInteger? StakingPoolDeactivationEpoch { get; set; }

        public string StakingPoolId { get; set; }

        public BigInteger StakingPoolSuiBalance { get; set; }

        public string SuiAddress { get; set; }

        public BigInteger VotingPower { get; set; }

        public string WorkerAddress { get; set; }

        public string WorkerPubkeyBytes { get; set; }
    }

}
