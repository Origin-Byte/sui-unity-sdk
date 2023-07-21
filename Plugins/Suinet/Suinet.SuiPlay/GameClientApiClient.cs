using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using Suinet.SuiPlay.DTO;
using Suinet.SuiPlay.Http;
using Suinet.SuiPlay.Requests;
using Suinet.SuiPlay.Responses;
using System.Text;
using System.Threading.Tasks;

namespace Suinet.SuiPlay
{
    public class GameClientApiClient
    {
        private readonly IHttpService _httpService;
        private readonly ITokenStorage _tokenStorage;
        private string _accessToken;
        
        public GameClientApiClient(IHttpService httpService, ITokenStorage tokenStorage)
        {
            _httpService = httpService;
            _tokenStorage = tokenStorage;
            // JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            // {
            //     ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            // };
            LoadToken();
        }

        public async Task<SuiPlayResult<Player>> GetPlayerProfileAsync(string gameId)
        {
            var response = await _httpService.GetAsync($"/client/profile?gameId={gameId}");
            return await HandleResponse<Player>(response);
        }

        public async Task<SuiPlayResult<Player>> UpdatePlayerProfileAsync(string gameId, Player player)
        {
            var content = new StringContent(JsonConvert.SerializeObject(player), Encoding.UTF8, "application/json");
            var response = await _httpService.PatchAsync($"/client/profile?gameId={gameId}", content);
            return await HandleResponse<Player>(response);
        }

        public async Task<SuiPlayResult<AuthResponse>> LoginWithEmailAsync(LoginRequest request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _httpService.PostAsync("/client/auth/login", content);
            var result = await HandleResponse<AuthResponse>(response);

            if (result.IsSuccess)
            {
                _accessToken = result.Value.IdToken;
                _httpService.SetAuthorizationHeader("Bearer", _accessToken);
                _tokenStorage.SaveToken(_accessToken);
            }

            return result;
        }

        public async Task<SuiPlayResult<RegistrationResponse>> RegisterWithEmailAsync(RegistrationRequest request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _httpService.PostAsync("/client/auth/register", content);
            return await HandleResponse<RegistrationResponse>(response);
        }

        public async Task<SuiPlayResult<AuthResponse>> LoginAnonymouslyAsync()
        {
            var response = await _httpService.PostAsync("/client/auth/login-anonymous", null);
            return await HandleResponse<AuthResponse>(response);
        }

        public async Task<SuiPlayResult<AuthResponse>> AddEmailPasswordAsync(RegistrationRequest request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _httpService.PostAsync("/client/auth/add-emailpassword", content);
            return await HandleResponse<AuthResponse>(response);
        }

        public async Task<SuiPlayResult<string>> ResetPasswordAsync(EmailData emailData)
        {
            var content = new StringContent(JsonConvert.SerializeObject(emailData), Encoding.UTF8, "application/json");
            var response = await _httpService.PostAsync("/client/auth/reset-password", content);
            return await HandleResponse<string>(response);
        }

        public async Task<SuiPlayResult<string>> VerifyEmailAsync(EmailData emailData)
        {
            var content = new StringContent(JsonConvert.SerializeObject(emailData), Encoding.UTF8, "application/json");
            var response = await _httpService.PostAsync("/client/auth/verify-email", content);
            return await HandleResponse<string>(response);
        }

        public async Task<SuiPlayResult<List<Wallet>>> ListWalletsAsync(string gameId)
        {
            var response = await _httpService.GetAsync($"/client/wallets?gameId={gameId}");
            return await HandleResponse<List<Wallet>>(response);
        }

        public async Task<SuiPlayResult<string>> GetWalletAsync(string gameId, string walletId)
        {
            var response = await _httpService.GetAsync($"/client/wallets/{walletId}?gameId={gameId}");
            return await HandleResponse<string>(response);
        }

        public async Task<SuiPlayResult<SuiTransactionResponseData>> ExecuteSponsoredTransactionAsync(string gameId, string walletId, SuiTransactionRequest request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _httpService.PostAsync($"/client/wallets/{walletId}/transactions/execute-sponsored?gameId={gameId}", content);
            return await HandleResponse<SuiTransactionResponseData>(response);
        }

        public async Task<SuiPlayResult<SignTransactionResult>> SignTransactionAsync(string gameId, string walletId, SignTransactionRequest transactionRequest)
        {
            var content = new StringContent(JsonConvert.SerializeObject(transactionRequest), Encoding.UTF8, "application/json");
            var response = await _httpService.PostAsync($"/client/wallets/{walletId}/transactions/sign?gameId={gameId}", content);
            return await HandleResponse<SignTransactionResult>(response);
        }
        
        private async Task<SuiPlayResult<T>> HandleResponse<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<T>(content);
                return new SuiPlayResult<T> { Value = result };
            }
            else
            {
                return new SuiPlayResult<T> { Error = $"Error {response.StatusCode}: {await response.Content.ReadAsStringAsync()}" };
            }
        }
        
        public void LoadToken()
        {
            _accessToken = _tokenStorage.LoadToken();
            if (!string.IsNullOrEmpty(_accessToken))
            {
                _httpService.SetAuthorizationHeader("Bearer", _accessToken);
            }
        }
    }

}
