using Newtonsoft.Json;
using Suinet.Rpc.Types.JsonConverters;

namespace Suinet.Rpc.Types
{
    public class SuiObject
    {
        [JsonConverter(typeof(DataJsonConverter))]

        public Data Data { get; set; }

        public Owner Owner { get; set; }

        public string PreviousTransaction { get; set; }

        public SuiObjectRef Reference { get; set; }

        public ulong StorageRebate { get; set; }
    }
}
