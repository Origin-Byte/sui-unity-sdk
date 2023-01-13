using System.Collections.Generic;

namespace Suinet.NftProtocol.Nft
{
    public class ArtNft : Nft
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public Dictionary<string,string> Attributes { get; set; }
    }
}
