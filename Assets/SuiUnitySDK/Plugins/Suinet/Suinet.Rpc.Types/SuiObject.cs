namespace Suinet.Rpc.Types
{
    public class SuiObject
    {
        public SuiData Data { get; set; }

        public SuiOwner Owner { get; set; }

        public string PreviousTransaction { get; set; }

        public SuiObjectRef Reference { get; set; }

        public ulong StorageRebate { get; set; }
    }
}
