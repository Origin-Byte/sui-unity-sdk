using System.Collections.Generic;

namespace Suinet.Rpc.Types.MoveTypes
{
    public class SuiProgrammableMoveCall
    {
        public List<SuiArgument> Arguments { get; set; }
        public string Function { get; set; }
        public string Module { get; set; }
        public string Package { get; set; }
        public List<string> TypeArguments { get; set; }
    }
}
