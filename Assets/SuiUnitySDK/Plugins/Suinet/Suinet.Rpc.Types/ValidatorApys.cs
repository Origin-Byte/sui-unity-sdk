using System.Collections.Generic;
using System.Numerics;

namespace Suinet.Rpc.Types
{
    public class ValidatorApy
    {
        public string Address { get; set; }
        public double Apy { get; set; }
    }

    public class ValidatorApys
    {
        public List<ValidatorApy> Apys { get; set; }
        public BigInteger Epoch { get; set; }
    }
}
