using System.Collections.Generic;

namespace Suinet.Rpc.Types
{
    public class Page_for_Event_and_EventID
    {
        public List<SuiEvent> Data { get; set; }

        public EventId NextCursor { get; set; }

        public bool HasNextPage { get; set; }
    }
}
