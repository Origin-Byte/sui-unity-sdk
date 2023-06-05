using Newtonsoft.Json;
using Suinet.Rpc.Types.JsonConverters;

namespace Suinet.Rpc.Types
{
    [JsonConverter(typeof(SuiAddressJsonConverter))]
    public class SuiAddress
    {
        private readonly string value;

        public SuiAddress(string value)
        {
            this.value = value;
        }

        public static implicit operator SuiAddress(string value)
        {
            return new SuiAddress(value);
        }

        public static implicit operator string(SuiAddress address)
        {
            return address.value;
        }
    }
}
