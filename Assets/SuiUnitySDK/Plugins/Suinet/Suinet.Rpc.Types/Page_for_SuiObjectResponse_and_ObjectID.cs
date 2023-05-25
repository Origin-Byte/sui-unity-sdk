using System.Collections.Generic;

namespace Suinet.Rpc.Types
{
    public class Page_for_SuiObjectResponse_and_ObjectID
    {
        public List<SuiObjectResponse> Data { get; set; }

        public bool HasNextPage { get; set; }

        public ObjectId NextCursor { get; set; }
    }
}
