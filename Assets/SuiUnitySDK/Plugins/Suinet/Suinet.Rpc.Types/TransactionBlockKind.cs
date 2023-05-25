using System.Collections.Generic;
using System.Numerics;
using Newtonsoft.Json;
using Suinet.Rpc.Types.MoveTypes;

namespace Suinet.Rpc.Types
{
    public enum TransactionBlockKindType
    {
        ChangeEpoch,
        Genesis,
        ConsensusCommitPrologue,
        ProgrammableTransaction
    }

    public abstract class TransactionBlockKind
    {
        public TransactionBlockKindType Kind { get; set; }
    }

    public class ChangeEpochTransactionBlockKind : TransactionBlockKind
    {
        public BigInteger ComputationCharge { get; set; }
        public BigInteger Epoch { get; set; }
        public BigInteger EpochStartTimestampMs { get; set; }
        public BigInteger StorageCharge { get; set; }
        public BigInteger StorageRebate { get; set; }

        public ChangeEpochTransactionBlockKind()
        {
            Kind = TransactionBlockKindType.ChangeEpoch;
        }
    }

    public class GenesisTransactionBlockKind : TransactionBlockKind
    {
        public List<string> Objects { get; set; }

        public GenesisTransactionBlockKind()
        {
            Kind = TransactionBlockKindType.Genesis;
        }
    }

    public class ConsensusCommitPrologueTransactionBlockKind : TransactionBlockKind
    {
        public BigInteger CommitTimestampMs { get; set; }
        public BigInteger Epoch { get; set; }
        public BigInteger Round { get; set; }

        public ConsensusCommitPrologueTransactionBlockKind()
        {
            Kind = TransactionBlockKindType.ConsensusCommitPrologue;
        }
    }

    public class ProgrammableTransactionBlockKind : TransactionBlockKind
    {
        [JsonProperty("inputs")]
        public List<SuiCallArg> Inputs { get; set; }

        [JsonProperty("transactions")]
        public List<SuiTransaction> Transactions { get; set; }

        public ProgrammableTransactionBlockKind()
        {
            Kind = TransactionBlockKindType.ProgrammableTransaction;
        }
    }
}
