using System.Collections.Generic;

namespace Suinet.Rpc.Types
{
    public class SuiPage_for_DynamicFieldInfo_and_ObjectID
    {
        public IEnumerable<SuiDynamicFieldInfo> Data { get; set; }

        public object NextCursor { get; set; }
    }
}
