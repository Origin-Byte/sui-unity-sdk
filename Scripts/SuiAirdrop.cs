using System.Threading.Tasks;
using Suinet.Faucet;
using UnityEngine;

public static class SuiAirdrop
{
    public static async Task RequestAirdrop(string address)
    {
        var faucet = new UnityWebRequestFaucetClient();
        var success = await faucet.AirdropGasAsync(address);
        
        Debug.Log($"Airdropped to {address}. Success: {success}");
    }
}
