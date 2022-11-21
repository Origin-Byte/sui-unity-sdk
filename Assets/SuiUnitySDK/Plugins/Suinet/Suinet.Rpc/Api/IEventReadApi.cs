using Suinet.Rpc.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Suinet.Rpc.Api
{
    public interface IEventReadApi
    {
        /// <summary>
        /// Return list of events for a specified query criteria.
        /// </summary>
        /// <param name="query">the event query criteria</param>
        /// <param name="cursor">optional paging cursor</param>
        /// <param name="limit">maximum number of items per page</param>
        /// <param name="query result ordering"></param>
        /// <returns></returns>
        Task<RpcResult<SuiPage_for_EventEnvelope_and_EventID>> GetEventsAsync(ISuiEventQuery query, SuiEventId cursor, ulong limit, bool descendingOrder = false);
    }
}
