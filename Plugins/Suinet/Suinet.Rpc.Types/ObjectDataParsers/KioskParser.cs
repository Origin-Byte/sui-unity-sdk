using Suinet.Rpc.Types.MoveTypes;
using System.Text.RegularExpressions;

namespace Suinet.Rpc.Types.ObjectDataParsers
{
    public class KioskParser : IObjectDataParser<Kiosk>
    {
        public Regex TypeRegex => new Regex(@"0x2::kiosk::Kiosk");

        public Kiosk Parse(ObjectData objectData)
        {
            var moveObjData = objectData.Content as MoveObjectData;
            var kiosk = moveObjData.ConvertFieldsTo<Kiosk>();
            return kiosk;
        }
    }
}
