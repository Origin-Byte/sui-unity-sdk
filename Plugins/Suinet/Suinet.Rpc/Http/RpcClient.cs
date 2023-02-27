
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Suinet.Rpc.JsonRpc;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Suinet.Rpc.Http
{
    public class RpcClient : IRpcClient
    {
        // TODO use httpclientfactory
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public Uri Endpoint { get; private set; }

        public RpcClient(string url, HttpClient httpClient = default, ILogger logger = default)
        {
            Endpoint = new Uri(url);
            _httpClient = httpClient ?? new HttpClient { BaseAddress = Endpoint };
            _logger = logger;
        }

        public async Task<RpcResult<T>> SendAsync<T>(JsonRpcRequest request)
        {
            var requestJson = JsonConvert.SerializeObject(request, new Newtonsoft.Json.Converters.StringEnumConverter());

            try
            {
                // create byte buffer to avoid charset=utf-8 in content-type header
                // as this is rejected by some RPC nodes
                var buffer = Encoding.UTF8.GetBytes(requestJson);
                using (var httpReq = new HttpRequestMessage(HttpMethod.Post, (string)null) {
                    Content = new ByteArrayContent(buffer)
                    {
                        Headers = {
                            { "Content-Type", "application/json"}
                        }
                    }
                })
                {
                    using (var response = await _httpClient.SendAsync(httpReq).ConfigureAwait(false))
                    {
                        var result = await HandleResult<T>(request, response).ConfigureAwait(false);
                        result.RawRpcRequest = requestJson;
                        return result;
                    }
                }
            }
            catch (HttpRequestException e)
            {
                var result = new RpcResult<T>
                {
                    IsSuccess = false,
                    RawRpcRequest = requestJson
                };
                _logger?.LogDebug(new EventId(request.Id, request.Method), $"Caught exception: {e.Message}");
                return result;
            }
            catch (Exception e)
            {
                var result = new RpcResult<T>
                {
                    ErrorMessage = e.Message,
                    RawRpcRequest = requestJson
                };
                _logger?.LogDebug(new EventId(request.Id, request.Method), $"Caught exception: {e.Message}");
                return result;
            }
        }

        private async Task<RpcResult<T>> HandleResult<T>(JsonRpcRequest req, HttpResponseMessage response)
        {
            RpcResult<T> result = new RpcResult<T>();
            try
            {
                result.RawRpcResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                _logger?.LogInformation(new EventId(req.Id, req.Method), $"Result: {result.RawRpcResponse}");
                var res = JsonConvert.DeserializeObject<JsonRpcValidResponse<T>>(result.RawRpcResponse);

                if (res != null && res.Result != null)
                {
                    result.Result = res.Result;
                    result.IsSuccess = true;
                }
                else
                {
                    var errorRes = JsonConvert.DeserializeObject<JsonRpcErrorResponse>(result.RawRpcResponse);
                    if (errorRes != null)
                    {
                        result.ErrorMessage = errorRes.Error.Message;
                    }
                    else
                    {
                        result.ErrorMessage = "Something wrong happened.";
                    }
                }
            }
            catch (JsonException e)
            {
                _logger?.LogDebug(new EventId(req.Id, req.Method), $"Caught exception: {e.Message}");
                result.IsSuccess = false;
                result.ErrorMessage = "Unable to parse json.";
            }

            return result;
        }
    }
}
