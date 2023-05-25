using System.Collections.Generic;

namespace Suinet.Rpc.Types.MoveTypes
{
    public class SuiMoveNormalizedStruct
    {
        public SuiMoveAbilitySet Abilities { get; set; }

        public List<SuiMoveNormalizedField> Fields { get; set; }

        public List<SuiMoveStructTypeParameter> TypeParameters { get; set; }
    }
}
