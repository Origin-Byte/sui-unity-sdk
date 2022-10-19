using System.Collections.Generic;

namespace Suinet.Rpc
{
    public static class ArgumentBuilder
    {
        public static IEnumerable<object> BuildArguments(params object[] @params)
        {
            return @params;
        }

        public static IEnumerable<string> BuildTypeArguments(params string[] @params)
        {
            return @params;
        }
    }
}
