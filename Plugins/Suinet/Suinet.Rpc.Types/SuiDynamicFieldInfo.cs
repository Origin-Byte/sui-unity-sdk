using Suinet.Rpc.Types.MoveTypes;

namespace Suinet.Rpc.Types
{
    public class SuiDynamicFieldInfo
    {
        public string Digest { get; set; }

        public string Name { get; set; }

        public string ObjectId { get; set; }

        public MoveType ObjectType { get; set; }

        public string Type { get; set; }

        public ulong Version { get; set; }
    }
}
