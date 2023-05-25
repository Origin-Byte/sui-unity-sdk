using System.Collections.Generic;

namespace Suinet.Rpc.Types.MoveTypes
{
    public class SuiMoveNormalizedModule
    {
        public string Address { get; set; }

        public Dictionary<string, SuiMoveNormalizedFunction> ExposedFunctions { get; set; }
        public uint FileFormatVersion { get; set; }

        public List<SuiMoveModuleId> Friends { get; set; }

        public string Name { get; set; }

        public Dictionary<string, SuiMoveNormalizedStruct> Structs { get; set; }
    }

}
