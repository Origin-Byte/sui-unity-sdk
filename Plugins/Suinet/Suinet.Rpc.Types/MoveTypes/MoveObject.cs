using System.Collections.Generic;

namespace Suinet.Rpc.Types.MoveTypes
{
    public class MoveObject
    {
        public MoveType Type { get; set; }
        public Dictionary<string, object> Fields { get; set; }
    }
}
