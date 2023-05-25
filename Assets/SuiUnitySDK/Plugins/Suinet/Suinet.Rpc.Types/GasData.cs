using System.Collections.Generic;
using System.Numerics;

namespace Suinet.Rpc.Types
{
    public class GasData
    {
        public BigInteger Budget { get; set; }

        public string Owner { get; set; }

        public List<SuiObjectRef> Payment { get; set; }

        public BigInteger Price { get; set; }
    }
}
