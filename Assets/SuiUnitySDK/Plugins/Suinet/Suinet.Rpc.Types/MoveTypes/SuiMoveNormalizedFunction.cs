using System.Collections.Generic;

namespace Suinet.Rpc.Types.MoveTypes
{
    public class SuiMoveNormalizedFunction
    {
        public bool IsEntry { get; set; }

        public List<SuiMoveNormalizedType> Parameters { get; set; }

        public List<SuiMoveNormalizedType> Return { get; set; }

        public List<SuiMoveAbilitySet> TypeParameters { get; set; }

        public SuiMoveVisibility Visibility { get; set; }
    }
}
