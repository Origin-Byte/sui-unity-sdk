using System.Collections.Generic;
using System.Numerics;

namespace Suinet.Rpc.Types
{
    public class TransactionBlockEffects
    {
        public List<SuiOwnedObjectRef> Created { get; set; }
        public List<SuiObjectRef> Deleted { get; set; }
        public List<string> Dependencies { get; set; }
        public string EventsDigest { get; set; }
        public BigInteger ExecutedEpoch { get; set; }
        public SuiOwnedObjectRef GasObject { get; set; }
        public GasCostSummary GasUsed { get; set; }
        public string MessageVersion { get; set; }
        public List<TransactionBlockEffectsModifiedAtVersions> ModifiedAtVersions { get; set; }
        public List<SuiOwnedObjectRef> Mutated { get; set; }
        public List<SuiObjectRef> SharedObjects { get; set; }
        public ExecutionStatusResponse Status { get; set; }
        public string TransactionDigest { get; set; }
        public List<SuiOwnedObjectRef> Unwrapped { get; set; }
        public List<SuiObjectRef> UnwrappedThenDeleted { get; set; }
        public List<SuiObjectRef> Wrapped { get; set; }
    }

}
