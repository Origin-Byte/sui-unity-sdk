namespace Suinet.Wallet
{
    public interface IKeyPair
    {
        string PublicKeyBase64 { get; }
        string PrivateKeyBase64 { get; }
        string PublicKeyAsSuiAddress { get; }

        byte[] PublicKey { get; }
        byte[] PrivateKey { get; }

        string ToSuiAddress(byte[] publicKeyBytes);
        string Sign(string base64message);
        byte[] Sign(byte[] message);

        SignatureScheme GetKeyScheme();
    }
}
