using System.Collections.Generic;
using System.Numerics;

namespace Suinet.Rpc.Types
{
    public class Checkpoint
    {
        public List<object> CheckpointCommitments { get; set; }

        public string Digest { get; set; }

        public object EndOfEpochData { get; set; }

        public BigInteger Epoch { get; set; }

        public GasCostSummary EpochRollingGasCostSummary { get; set; }

        public BigInteger NetworkTotalTransactions { get; set; }

        public string PreviousDigest { get; set; }

        public BigInteger SequenceNumber { get; set; }

        public BigInteger TimestampMs { get; set; }

        public List<string> Transactions { get; set; }

        public string ValidatorSignature { get; set; } // Assuming Base64 is a string representation
    }
}
