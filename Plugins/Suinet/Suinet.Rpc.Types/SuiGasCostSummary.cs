namespace Suinet.Rpc.Types
{
    public class SuiGasCostSummary
    {
        public ulong ComputationCost { get; set; }
        public ulong StorageCost { get; set; }
        public ulong StorageRebate { get; set; }
    }
}
