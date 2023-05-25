using Newtonsoft.Json;
using Suinet.Rpc.Types.JsonConverters;
using System.Text.RegularExpressions;

namespace Suinet.Rpc.Types.MoveTypes
{
    [JsonConverter(typeof(MoveTypeConverter))]
    public class MoveType
    {
        private static readonly Regex TypeRegex = new Regex("(0x[a-f0-9]{1,40})::(.[^::]*)::(.*)");

        public string Type { get; set; }

        public string PackageId { get; set; }

        public string Module { get; set; }

        public string Struct { get; set; }

        public MoveType(string typeString) 
        {
            Type = typeString;
            var match = TypeRegex.Match(typeString);

            if (match.Success && match.Groups.Count == 4)
            {
                PackageId = match.Groups[1].Value;
                Module = match.Groups[2].Value;
                Struct = match.Groups[3].Value;
            }
        }
    }
}
