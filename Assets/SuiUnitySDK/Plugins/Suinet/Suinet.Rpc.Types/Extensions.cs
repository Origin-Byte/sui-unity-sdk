using Newtonsoft.Json;
using System.Collections.Generic;

namespace Suinet.Rpc.Types
{
    public static class Extensions
    {
        public static T ToObject<T>(this Dictionary<string, object> dict)
        {
            var jsonString = JsonConvert.SerializeObject(dict);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}
