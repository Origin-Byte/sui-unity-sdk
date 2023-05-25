using System.Collections.Generic;
using System.Numerics;

namespace Suinet.Rpc.Types
{
    public class Page_for_Checkpoint_and_BigInteger
    {
        public List<Checkpoint> Data { get; set; }

        public bool HasNextPage { get; set; }

        public BigInteger? NextCursor { get; set; }
    }
}
