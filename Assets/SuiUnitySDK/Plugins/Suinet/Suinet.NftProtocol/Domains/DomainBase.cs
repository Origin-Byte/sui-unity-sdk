using Newtonsoft.Json;
using Suinet.Rpc.Types.MoveTypes;
using System.Collections.Generic;

namespace Suinet.NftProtocol.Domains
{
    public class DomainBase
    {
        public UID Id { get; set; }

        public Name Name { get; set; }

        public Value Value { get; set; }

        public T GetTypedField<T>(string fieldName) where T : class
        {
            return Value.Fields[fieldName] as T;
        }
    }

    public class Name
    {
        public string Type { get; set; }

        public Fields Fields { get; set; }
    }

    public class Fields
    {
        [JsonProperty("dummy_field")]
        public bool DummyField { get; set; }
    }

    public class Value
    {
        public string Type { get; set; }
        public Dictionary<string, object> Fields { get; set; }
    }
}
