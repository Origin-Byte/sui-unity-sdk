using Newtonsoft.Json;
using Suinet.Rpc.Types.Converters;

namespace Suinet.Rpc.Types
{
    [JsonConverter(typeof(SuiOwnerConverter))]
    public class SuiOwner
    {
        public SuiOwnerType Type { get; }

        public string Address { get; }

        public SuiOwner(SuiOwnerType type)
        {
            Type = type;
        }

        public SuiOwner(SuiOwnerType type, string address)
        {
            Type = type;
            Address = address;
        }
    }
}
