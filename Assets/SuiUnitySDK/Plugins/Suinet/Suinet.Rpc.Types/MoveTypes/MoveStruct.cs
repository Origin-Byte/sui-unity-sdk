using Newtonsoft.Json;
using Suinet.Rpc.Types.JsonConverters;
using System;
using System.Collections.Generic;

namespace Suinet.Rpc.Types.MoveTypes
{
    [JsonConverter(typeof(MoveStructJsonConverter))]
    public abstract class MoveStruct
    {
        public virtual T ToObject<T>()
        {
            return Serialize<MoveStruct, T>(this);
        }

        public static T Serialize<U, T>(U obj) where U : MoveStruct
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(obj);
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }

    public class ArrayMoveStruct : MoveStruct
    {
        public List<MoveValue> Value { get; set; }

        public override T ToObject<T>()
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(Value);
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (Exception)
            {
            }
            return default;
        }
    }

    public class ObjectMoveStruct : MoveStruct
    {
        public Dictionary<string, MoveValue> Fields { get; set; }
        public string Type { get; set; }

        public override T ToObject<T>()
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(Fields);
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (Exception)
            {
            }
            return default;
        }
    }

    public class AdditionalPropertiesMoveStruct : MoveStruct
    {
        public Dictionary<string, MoveValue> AdditionalProperties { get; set; }

        public override T ToObject<T>()
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(AdditionalProperties);
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (Exception e)
            {
                var a = 34;
            }
            return default;
        }
    }
}
