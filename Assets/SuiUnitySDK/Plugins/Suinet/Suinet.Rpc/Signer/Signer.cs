using Suinet.Rpc.Client;
using Suinet.Rpc.Types;
using Suinet.Wallet;
using System;
using System.Threading.Tasks;

namespace Suinet.Rpc.Signer
{
    public class Signer : ISigner
    {
        private readonly IJsonRpcApiClient _rpcApiClient;
        private readonly IKeyPair _keyPair;

        public Signer(IJsonRpcApiClient rpcApiClient, IKeyPair keyPair)
        {
            _rpcApiClient = rpcApiClient;
            _keyPair = keyPair;
        }

        public async Task<RpcResult<TransactionBlockResponse>> SignAndExecuteAsync(Func<Task<RpcResult<TransactionBlockBytes>>> method,
            ExecuteTransactionRequestType txRequestType)
        {
            //var buildTxCallResult = await method();
            //if (buildTxCallResult.IsSuccess)
            //{
            //    var txBytes = buildTxCallResult.Result.TxBytes;
            //    var signature = _keyPair.Sign(txBytes);

            //    var txResponse = await _rpcApiClient.ExecuteTransactionAsync(txBytes, SuiSignatureScheme.ED25519, signature, _keyPair.PublicKeyBase64, txRequestType);
            //    return txResponse;
            //}

            return null;
        }
    }
}
