using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Suinet.Rpc.Types.MoveTypes
{
    public static class MoveTypeHelper
    {
        public static bool IsMatchingMoveType(Type type, string typeString)
        {
            var moveType = type.GetCustomAttribute<MoveTypeAttribute>();
            if (moveType == null)
            {
                return false;
            }

            if (string.Equals(moveType.Type, typeString, StringComparison.InvariantCulture))
            {
                return true;
            }

            var regex = new Regex(moveType.Type);
            return regex.IsMatch(typeString);
        }
    }
}
