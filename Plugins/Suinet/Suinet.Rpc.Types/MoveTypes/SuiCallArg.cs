using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Suinet.Rpc.Types.JsonConverters;
using System.Numerics;

namespace Suinet.Rpc.Types.MoveTypes
{
    [JsonConverter(typeof(StringEnumConverter), converterParameters: typeof(CamelCaseNamingStrategy))]
    public enum SuiObjectType
    {
        ImmOrOwnedObject,
        SharedObject
    }

    [JsonConverter(typeof(StringEnumConverter), converterParameters: typeof(CamelCaseNamingStrategy))]
    public enum SuiCallArgType
    {
        Object,
        Pure
    }

    [JsonConverter(typeof(SuiCallArgConverter))]
    public abstract class SuiCallArg
    {
        [JsonProperty("type")]
        public SuiCallArgType Type { get; set; }
    }

    public class ObjectSuiCallArg : SuiCallArg
    {
        public ObjectProperties Properties { get; set; }
    }

    public abstract class ObjectProperties
    {
        public SuiObjectType ObjectType { get; set; }

        public ObjectId ObjectId { get; set; }
    }

    public class ImmOrOwnedObjectProperties : ObjectProperties
    {
        public string Digest { get; set; }

        public BigInteger Version { get; set; }

        public ImmOrOwnedObjectProperties()
        {
            ObjectType = SuiObjectType.ImmOrOwnedObject;
        }
    }

    public class SharedObjectProperties : ObjectProperties
    {
        public BigInteger InitialSharedVersion { get; set; }

        public bool Mutable { get; set; }

        public SharedObjectProperties()
        {
            ObjectType = SuiObjectType.SharedObject;
        }
    }

    public class PureSuiCallArg : SuiCallArg
    {
        [JsonProperty("value")]
        public object Value { get; set; }

        [JsonProperty("valueType")]
        public string ValueType { get; set; }
    }
}
