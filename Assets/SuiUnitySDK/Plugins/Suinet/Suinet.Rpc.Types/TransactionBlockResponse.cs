using System.Collections.Generic;
using System.Numerics;

namespace Suinet.Rpc.Types
{
    public class TransactionBlockResponse
    {
        public List<BalanceChange> BalanceChanges { get; set; }

        public TransactionBlock Transaction { get; set; }

        public string RawTransaction { get; set; }

        public TransactionBlockEffects Effects { get; set; }

        public List<SuiEvent> Events { get; set; }

        public ulong TimestampMs { get; set; }

        public BigInteger? Checkpoint { get; set; }

        public bool? ConfirmedLocalExecution { get; set; }

        public string Digest { get; set; }

        public List<ObjectChange> ObjectChanges { get; set; }

        public List<string> Errors { get; set; }
    }
}
