using System.Threading.Tasks;

namespace Suinet.Faucet
{
    public interface IFaucetClient
    {
        Task<bool> AirdropGasAsync(string recipient);
    }
}
