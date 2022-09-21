namespace Suinet.Rpc
{
    public class RpcResult<T>
    {
        public bool IsSuccess { get; set; }
        public T Result { get; set; }
        public string RawRpcRequest { get; set; }
        public string RawRpcResponse { get; set; }
        public string ErrorMessage { get; set; }
    }
}
