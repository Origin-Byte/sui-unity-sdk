using System.Collections.Generic;

namespace Suinet.Rpc
{
    public static class SuiJsonSanitizer
    {
        public static IEnumerable<object> SanitizeArguments(IEnumerable<object> arguments)
        {
            var paramList = new List<object>();
            foreach (var argument in arguments)
            {
                var paramType = argument.GetType();
                if (paramType == typeof(ulong) || paramType == typeof(long))
                {
                    paramList.Add(argument.ToString());
                }
                else
                {
                    paramList.Add(argument);
                }
            }

            return paramList;
        }

    }
}
