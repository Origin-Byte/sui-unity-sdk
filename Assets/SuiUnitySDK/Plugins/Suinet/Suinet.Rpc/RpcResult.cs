namespace Suinet.Rpc
{
    public class RpcResult<T>
    {
        public bool IsSuccess { get; set; }
        public T Result { get; set; }
        public string RawRpcRequest { get; internal set; }
        public string RawRpcResponse { get; internal set; }
        public string ErrorMessage { get; set; }
    }
}
