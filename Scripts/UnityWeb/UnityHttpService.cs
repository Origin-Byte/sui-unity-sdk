using System.Net;
using System.Net.Http;
using UnityEngine.Networking;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using Suinet.SuiPlay.Http;

public class UnityHttpService : IHttpService
{
    private readonly string _baseUrl;

    public UnityHttpService(string baseUrl)
    {
        _baseUrl = baseUrl;
    }
    
    public async Task<HttpResponseMessage> GetAsync(string url)
    {
        Debug.Log($"UnityHttpService.GetAsync url: {url}");

        using var unityWebRequest = UnityWebRequest.Get(_baseUrl + url);
        unityWebRequest.SendWebRequest();
        while (!unityWebRequest.isDone)
        {
            await Task.Yield();
        }
            
        return HandleResult(unityWebRequest);
    }

    public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
    {
        var contentString = await content.ReadAsStringAsync();
        Debug.Log($"UnityHttpService.PostAsync url: {url}, content: {contentString}");
        
        var contentBytes = await content.ReadAsByteArrayAsync();
        using var unityWebRequest = new UnityWebRequest(_baseUrl + url, "POST");
        unityWebRequest.uploadHandler = new UploadHandlerRaw(contentBytes);
        unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
        unityWebRequest.SetRequestHeader("Content-Type", "application/json");
        unityWebRequest.SendWebRequest();
        while (!unityWebRequest.isDone)
        {
            await Task.Yield();
        }
            
        return HandleResult(unityWebRequest);
    }

    public async Task<HttpResponseMessage> PatchAsync(string url, HttpContent content)
    {
        var contentString = await content.ReadAsStringAsync();
        Debug.Log($"UnityHttpService.PatchAsync url: {url}, content: {contentString}");

        var contentBytes = await content.ReadAsByteArrayAsync();
        using var unityWebRequest = new UnityWebRequest(_baseUrl + url, "PATCH");
        unityWebRequest.uploadHandler = new UploadHandlerRaw(contentBytes);
        unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
        unityWebRequest.SetRequestHeader("Content-Type", "application/json");
        while (!unityWebRequest.isDone)
        {
            await Task.Yield();
        }
            
        return HandleResult(unityWebRequest);
    }

    private HttpResponseMessage HandleResult(UnityWebRequest unityWebRequest)
    {
        Debug.Log($"UnityWebRequest response: {unityWebRequest.downloadHandler.text}");
        if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError || unityWebRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"UnityWebRequest error: {unityWebRequest.error}");
            return new HttpResponseMessage()
            {
                StatusCode = (HttpStatusCode)unityWebRequest.responseCode,
                Content = new StringContent(unityWebRequest.downloadHandler.text, Encoding.UTF8, "application/json")
            };
        }
        else
        {
            return new HttpResponseMessage()
            {
                StatusCode = (HttpStatusCode)unityWebRequest.responseCode,
                Content = new StringContent(unityWebRequest.downloadHandler.text, Encoding.UTF8, "application/json")
            };
        }
    }
}
