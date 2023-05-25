using System.Numerics;

namespace Suinet.Rpc.Types
{
    public class GasCostSummary
    {
        public BigInteger ComputationCost { get; set; }
        public BigInteger NonRefundableStorageFee { get; set; }
        public BigInteger StorageCost { get; set; }
        public BigInteger StorageRebate { get; set; }
    }
}
