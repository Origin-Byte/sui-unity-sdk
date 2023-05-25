using Suinet.NftProtocol;
using Suinet.Rpc;
using Suinet.Rpc.Client;
using Suinet.Rpc.Signer;
using Suinet.Rpc.Types;
using Suinet.Wallet;

public static class SuiApi
{
    public static IJsonRpcApiClient Client { get; }
    public static INftProtocolClient NftProtocolClient { get; private set; }

    private static IKeyPair _signerKeyPair;
    private static ISigner _signer;
    public static ISigner Signer => GetSignerForActiveKeyPair();

    static SuiApi()
    {
        var rpcClient = new UnityWebRequestRpcClient(SuiConstants.TESTNET_FULLNODE);
        Client = new SuiJsonRpcApiClient(rpcClient);
        _signer = CreateSigner(Client, SuiWallet.GetActiveKeyPair());
        NftProtocolClient = new NftProtocolClient(Client, SuiWallet.GetActiveKeyPair());
    }

    private static ISigner GetSignerForActiveKeyPair()
    {
        var activeKeyPair = SuiWallet.GetActiveKeyPair();
        if (_signerKeyPair == activeKeyPair && _signer != null) return _signer;
        
        _signer = CreateSigner(Client, activeKeyPair);
        NftProtocolClient = new NftProtocolClient(Client, SuiWallet.GetActiveKeyPair());
        _signerKeyPair = activeKeyPair;

        return _signer;
    }
    
    private static ISigner CreateSigner(IJsonRpcApiClient apiClient, IKeyPair keyPair)
    {
        _signerKeyPair = keyPair;
        var signer = new Signer(apiClient, keyPair);
        return signer;
    }
}
