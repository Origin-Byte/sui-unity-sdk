using Newtonsoft.Json;
using Suinet.Rpc.Types.JsonConverters;
using System.Collections.Generic;

namespace Suinet.Rpc.Types.MoveTypes
{
    [JsonConverter(typeof(MoveValueJsonConverter))]
    public abstract class MoveValue
    {
    }

    public class IntegerMoveValue : MoveValue
    {
        public uint Value { get; set; }
    }

    public class BooleanMoveValue : MoveValue
    {
        public bool Value { get; set; }
    }

    public class StringMoveValue : MoveValue
    {
        public string Value { get; set; }
    }

    public class SuiAddressMoveValue : MoveValue
    {
        public string Value { get; set; }
    }

    public class ObjectIDMoveValue : MoveValue
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class MoveStructMoveValue : MoveValue
    {
        [JsonConverter(typeof(MoveValueJsonConverter))]
        public MoveStruct Value { get; set; }
    }

    public class ArrayMoveValue : MoveValue
    {
        [JsonConverter(typeof(MoveValueJsonConverter))]
        public List<MoveValue> Value { get; set; }
    }
}
