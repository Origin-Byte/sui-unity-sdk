namespace Suinet.Rpc.JsonRpc
{
    public class JsonRpcValidResponse<T> : JsonRpcObjectBase
    {
        public T Result { get; set; }
    }
}
