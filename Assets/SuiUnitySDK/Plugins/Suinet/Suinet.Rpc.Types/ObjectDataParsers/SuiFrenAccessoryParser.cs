using Suinet.Rpc.Types.Nfts;
using System.Text.RegularExpressions;

namespace Suinet.Rpc.Types.ObjectDataParsers
{
    public class SuiFrenAccessoryParser : IObjectDataParser<SuiFrenAccessory>
    {
        public Regex TypeRegex => new Regex("");

        public SuiFrenAccessory Parse(ObjectData objectData)
        {
            var moveData = objectData.Content as MoveObjectData;
            return moveData.ConvertFieldsTo<SuiFrenAccessory>();
        }
    }
}
