namespace Suinet.Rpc.JsonRpc
{
    public class JsonRpcErrorResponse : JsonRpcObjectBase
    {
        public JsonRpcError Error { get; set; }
    }
}
