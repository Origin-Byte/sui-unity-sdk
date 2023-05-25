using System.Collections.Generic;
using System.Numerics;

namespace Suinet.Rpc.Types
{
    public class Balance
    {
        public uint CoinObjectCount { get; set; }

        public string CoinType { get; set; }

        public Dictionary<string, BigInteger> LockedBalance { get; set; }

        public BigInteger TotalBalance { get; set; }
    }
}
