using System.Net.Http;
using System.Threading.Tasks;

namespace Suinet.SuiPlay.Http
{
    public interface IHttpService
    {
        Task<HttpResponseMessage> GetAsync(string url);
        Task<HttpResponseMessage> PostAsync(string url, HttpContent content);
        Task<HttpResponseMessage> PatchAsync(string url, HttpContent content);
        void SetAuthorizationHeader(string scheme, string parameter);
    }
}
