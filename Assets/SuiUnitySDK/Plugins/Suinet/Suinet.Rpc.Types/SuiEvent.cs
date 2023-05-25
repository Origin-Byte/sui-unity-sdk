using System.Numerics;

namespace Suinet.Rpc.Types
{
    public class SuiEvent
    {
        public string Bcs { get; set; }

        public string Id { get; set; }

        public ObjectId PackageId { get; set; }

        public string ParsedJson { get; set; }

        public SuiAddress Sender { get; set; }

        public BigInteger TimestampMs { get; set; }

        public string TransactionModule { get; set; }

        public string Type { get; set; }
    }

}