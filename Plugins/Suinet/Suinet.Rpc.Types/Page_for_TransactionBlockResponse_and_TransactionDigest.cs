using System.Collections.Generic;

namespace Suinet.Rpc.Types
{
    public class Page_for_TransactionBlockResponse_and_TransactionDigest
    {
        public List<TransactionBlockResponse> Data { get; set; }
        public bool HasNextPage { get; set; }
        public string NextCursor { get; set; }

    }
}
