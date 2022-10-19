using System.Collections.Generic;

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
        public ulong GasBudget { get; set; }
        public SuiExecuteTransactionRequestType RequestType { get; set; }
    }
}
