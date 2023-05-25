using Suinet.Rpc.Types.Coins;
using System.Text.RegularExpressions;

namespace Suinet.Rpc.Types.ObjectDataParsers
{
    public class SUICoinParser : IObjectDataParser<SUICoin>
    {
        public Regex TypeRegex => new Regex(@"^0x2::coin::Coin<0x2::sui::SUI>$");

        public SUICoin Parse(ObjectData objectData)
        {
            var moveContent = objectData.Content as MoveObjectData;
            return moveContent.ConvertFieldsTo<SUICoin>();
        }
    }
}
