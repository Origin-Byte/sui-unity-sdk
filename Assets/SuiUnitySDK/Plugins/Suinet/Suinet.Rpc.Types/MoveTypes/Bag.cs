using Newtonsoft.Json;

namespace Suinet.Rpc.Types.MoveTypes
{
    [MoveType("0x2::bag::Bag")]
    public class Bag
    {
       [JsonProperty("bag")]
       public BagData BagData { get; set; }
    }

    public class BagData
    {
        public string Type { get; set; }

        public BagFields Fields { get; set; }
    }

    public class BagFields
    {
        public UID Id { get; set; }
}
}
