using System.Collections.Generic;

namespace Suinet.Rpc.Types.MoveTypes
{
    public class MoveType
    {
        public string Type { get; set; }

        public Dictionary<string, object> Fields { get; set; }
    }
}
