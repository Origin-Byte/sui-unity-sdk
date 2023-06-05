using Newtonsoft.Json;
using Suinet.Rpc.Types.JsonConverters;

namespace Suinet.Rpc.Types
{
    [JsonConverter(typeof(ObjectIdJsonConverter))]
    public class ObjectId
    {
        private readonly string value;

        public ObjectId(string value)
        {
            this.value = value;
        }

        public static implicit operator ObjectId(string value)
        {
            return new ObjectId(value);
        }

        public static implicit operator string(ObjectId objectId)
        {
            return objectId.value;
        }

        public static bool IsValid(string input)
        {
            if (input.StartsWith("0x"))
            {
                input = input.Substring(2);
            }

            if (input.Length != 64) // In C#, a byte is represented by 2 hexadecimal digits
            {
                return false;
            }

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                bool isHexDigit = (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F');

                if (!isHexDigit)
                {
                    return false;
                }
            }

            return true;
        }

    }
}
