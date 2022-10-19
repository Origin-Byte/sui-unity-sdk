namespace Suinet.Rpc.Types
{
    public enum SuiExecutionStatus
    {
        Success,
        Failure,
    }

    public class SuiExecutionStatusObject
    {
        public SuiExecutionStatus Status { get; set; }
        public string Error { get; set; }
    }
}
