using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Suinet.Rpc.Types
{
    public static class Extensions
    {
        public static T ToObject<T>(this Dictionary<string, object> dict)
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(dict);
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch(Exception)
            {
            }
            return default;
        }

        public static T ToObject<T>(this Dictionary<string, string> dict)
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(dict);
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (Exception)
            {
            }
            return default;
        }
    }
}
