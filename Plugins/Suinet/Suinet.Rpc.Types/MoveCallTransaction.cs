using System.Collections.Generic;
using System.Numerics;

namespace Suinet.Rpc.Types
{
    public class MoveCallTransaction
    {
        public string Signer { get; set; }
        public string PackageObjectId { get; set; }
        public string Module { get; set; }
        public string Function { get; set; }
        public IEnumerable<string> TypeArguments { get; set; }
        public IEnumerable<object> Arguments { get; set; }
        public string Gas { get; set; }
        public BigInteger GasBudget { get; set; }
        public ExecuteTransactionRequestType RequestType { get; set; }
    }
}
