using Suinet.Rpc.Types.MoveTypes;
namespace Suinet.Rpc.Types
{
    public class SuiObjectInfo
    {
        public string ObjectId { get; set; }

        public ulong Version { get; set; }

        public string Digest { get; set; }

        public MoveType Type { get; set; }

        public SuiOwner Owner { get; set; }

        public string PreviousTransaction { get; set; }
    }
}
