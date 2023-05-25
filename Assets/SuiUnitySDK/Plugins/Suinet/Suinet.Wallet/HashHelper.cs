using Org.BouncyCastle.Crypto.Digests;

namespace Suinet.Wallet
{
    public static class HashHelper
    {
        public static byte[] ComputeBlake2bHash(byte[] data)
        {
            var hashAlgorithm = new Blake2bDigest(256);
            hashAlgorithm.BlockUpdate(data, 0, data.Length);
            byte[] digest = new byte[32];
            hashAlgorithm.DoFinal(digest, 0);

            return digest;
        }
    }
}
