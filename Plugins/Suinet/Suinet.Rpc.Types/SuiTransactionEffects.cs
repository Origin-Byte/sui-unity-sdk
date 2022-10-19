using System;
using System.Collections.Generic;

namespace Suinet.Rpc.Types
{
    public class SuiTransactionEffects
    {
        public IEnumerable<SuiOwnedObjectRef> Created { get; set; }   
        
        public IEnumerable<SuiOwnedObjectRef> Deleted { get; set; }   
        
        public IEnumerable<string> Dependencies { get; set; }

        public IEnumerable<object> Events { get; set; }

        public SuiOwnedObjectRef GasObject { get; set; }

        public SuiGasCostSummary GasUsed { get; set; }

        public IEnumerable<SuiOwnedObjectRef> Mutated { get; set; }

        public IEnumerable<SuiOwnedObjectRef> SharedObjects { get; set; }

        public string TransactionDigest { get; set; }

        public SuiExecutionStatusObject Status { get; set; }

        public IEnumerable<SuiOwnedObjectRef> Unwrapped { get; set; }

        public IEnumerable<SuiOwnedObjectRef> Wrapped { get; set; }
    }
}
