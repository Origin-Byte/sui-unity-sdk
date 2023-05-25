using Suinet.Rpc.Types;
using Suinet.Rpc.Types.ObjectDataParsers;
using System;
using System.Text.RegularExpressions;

namespace Suinet.NftProtocol.Nft
{
    public class ArtNftParser : IObjectDataParser<ArtNft>
    {
        public Regex TypeRegex => new Regex(@"^0x[a-fA-F0-9]{64}::[^:]+::[^:]+$");

        public ArtNft Parse(ObjectData objectData)
        {
            var moveContent = objectData.Content as MoveObjectData;
            var artNft = moveContent.ConvertFieldsTo<ArtNft>();
            return artNft;
        }
    }
}
