using System.Collections.Generic;

namespace Suinet.Rpc.JsonRpc
{
    public class JsonRpcRequest : JsonRpcObjectBase
    {
        public string Method { get; }

        public IEnumerable<object> Params { get; }

        public JsonRpcRequest(string method, IEnumerable<object> @params, int id = 1)
        {
            Method = method;
            Params = @params;
            Id = id;
        }
    }
}
