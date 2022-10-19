namespace Suinet.Rpc.Types
{
    public enum SuiExecuteTransactionRequestType
    {
        None = 0,
        ImmediateReturn = 1,
        WaitForTxCert = 2,
        WaitForEffectsCert = 3,
        //WaitForLocalExecution = 4
    }
}
