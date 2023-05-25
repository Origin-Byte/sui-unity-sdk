using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Suinet.Faucet
{
    public class FaucetClient : IFaucetClient
    {
        public async Task<bool> AirdropGasAsync(string recipient)
        {
            var httpClient = new HttpClient();
            var airdropRequest = new AirdropRequest(recipient);
            var json = JsonConvert.SerializeObject(airdropRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("https://faucet.testnet.sui.io/gas", content);
            return response.IsSuccessStatusCode;
        }
    }
}
