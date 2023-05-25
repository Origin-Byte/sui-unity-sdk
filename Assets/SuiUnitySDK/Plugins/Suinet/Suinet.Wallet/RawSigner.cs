using Chaos.NaCl;

namespace Suinet.Wallet
{
    public class RawSigner
    {
        private readonly IKeyPair _keypair;

        public RawSigner(IKeyPair keypair)
        {
            _keypair = keypair;
        }

        public string GetAddress()
        {
            return _keypair.PublicKeyAsSuiAddress;
        }

        public SerializedSignature SignMessage(string message)
        {
            return SignData(CryptoBytes.FromBase64String(message));
        }

        public SerializedSignature SignData(byte[] data)
        {
            var digest = HashHelper.ComputeBlake2bHash(data);
            var signature = _keypair.Sign(digest);
            var signatureScheme = _keypair.GetKeyScheme();

            return SignatureUtils.ToSerializedSignature(new SignaturePubkeyPair
            {
                SignatureScheme = signatureScheme,
                Signature = signature,
                PubKey = _keypair.PublicKeyBase64
            });
        }
    }
}
