using System.Collections.Generic;
using System.Numerics;

namespace Suinet.Rpc.Types
{
    public class SuiSystemStateSummary
    {
        public List<SuiValidatorSummary> ActiveValidators { get; set; }

        public List<List<AtRiskValidator>> AtRiskValidators { get; set; }

        public BigInteger Epoch { get; set; }

        public BigInteger EpochDurationMs { get; set; }

        public BigInteger EpochStartTimestampMs { get; set; }

        public string InactivePoolsId { get; set; }

        public BigInteger InactivePoolsSize { get; set; }

        public BigInteger MaxValidatorCount { get; set; }

        public BigInteger MinValidatorJoiningStake { get; set; }

        public string PendingActiveValidatorsId { get; set; }

        public BigInteger PendingActiveValidatorsSize { get; set; }

        public List<BigInteger> PendingRemovals { get; set; }

        public BigInteger ProtocolVersion { get; set; }

        public BigInteger ReferenceGasPrice { get; set; }

        public bool SafeMode { get; set; }

        public BigInteger SafeModeComputationRewards { get; set; }

        public BigInteger SafeModeNonRefundableStorageFee { get; set; }

        public BigInteger SafeModeStorageRebates { get; set; }

        public BigInteger SafeModeStorageRewards { get; set; }

        public BigInteger StakeSubsidyBalance { get; set; }

        public BigInteger StakeSubsidyCurrentDistributionAmount { get; set; }

        public int StakeSubsidyDecreaseRate { get; set; }

        public BigInteger StakeSubsidyDistributionCounter { get; set; }

        public BigInteger StakeSubsidyPeriodLength { get; set; }

        public BigInteger StakeSubsidyStartEpoch { get; set; }

        public string StakingPoolMappingsId { get; set; }

        public BigInteger StakingPoolMappingsSize { get; set; }

        public BigInteger StorageFundNonRefundableBalance { get; set; }

        public BigInteger StorageFundTotalObjectStorageRebates { get; set; }

        public BigInteger SystemStateVersion { get; set; }

        public BigInteger TotalStake { get; set; }

        public string ValidatorCandidatesId { get; set; }

        public BigInteger ValidatorCandidatesSize { get; set; }

        public BigInteger ValidatorLowStakeGracePeriod { get; set; }

        public BigInteger ValidatorLowStakeThreshold { get; set; }

        public List<List<ValidatorReportRecord>> ValidatorReportRecords { get; set; }

        public BigInteger ValidatorVeryLowStakeThreshold { get; set; }
    }

    public class AtRiskValidator
    {
        public string SuiAddress { get; set; }

        public BigInteger BigInteger { get; set; }
    }

    public class ValidatorReportRecord
    {
        public string SuiAddress { get; set; }

        public List<string> SuiAddresses { get; set; }
    }

}
