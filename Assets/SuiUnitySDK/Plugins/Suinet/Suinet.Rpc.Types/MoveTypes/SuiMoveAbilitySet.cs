using System.Collections.Generic;

namespace Suinet.Rpc.Types.MoveTypes
{
    public enum SuiMoveAbility
    {
        Copy,
        Drop,
        Store,
        Key
    }

    public class SuiMoveAbilitySet
    {
        public List<SuiMoveAbility> Abilities { get; set; }
    }
}
