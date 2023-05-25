using Suinet.Rpc.Types.Nfts;
using System.Text.RegularExpressions;

namespace Suinet.Rpc.Types.ObjectDataParsers
{
    public class CapySuiFrenParser : IObjectDataParser<CapySuiFren>
    {
        // TODO support chained / nested dynamic obj parsing
        //private IObjectDataParser<SuiFrenAccessory> _accessoryParser;

        //public CapySuiFrenParser(IObjectDataParser<SuiFrenAccessory> accessoryParser)
        //{
        //    _accessoryParser = accessoryParser;
        //}

        public Regex TypeRegex => new Regex(@"0x[a-fA-F0-9]{64}::\w+::\w+<0x[a-fA-F0-9]{64}::\w+::\w+>");

        public CapySuiFren Parse(ObjectData objectData)
        {
            var moveObjData = objectData.Content as MoveObjectData;
            var fren = new CapySuiFren()
            {
                ObjectId = objectData.ObjectId,
                Type = objectData.Type,
                Display = objectData.Display?.Data?.ToObject<DisplayData>(),
                Properties = moveObjData?.ConvertFieldsTo<CapySuiFrenProperties>(),
            };

            return fren;
        }
    }
}
