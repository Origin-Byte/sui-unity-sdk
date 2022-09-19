namespace Suinet.Rpc.Types
{
    public class SuiObject
    {
        public object Data { get; set; }

        public object Owner { get; set; }

        public string PreviousTransaction { get; set; }

        public SuiObjectRef Reference { get; set; }

        public ulong StorageRebate { get; set; }
    }
}
