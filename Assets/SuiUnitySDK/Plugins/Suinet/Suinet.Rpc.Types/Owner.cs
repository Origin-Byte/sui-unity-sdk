using Newtonsoft.Json;
using Suinet.Rpc.Types.JsonConverters;

namespace Suinet.Rpc.Types
{
    [JsonConverter(typeof(SuiOwnerConverter))]
    public class Owner
    {
        public SuiOwnerType Type { get; }

        public string Address { get; }

        public Owner(SuiOwnerType type)
        {
            Type = type;
        }

        public Owner(SuiOwnerType type, string address)
        {
            Type = type;
            Address = address;
        }
    }
}
