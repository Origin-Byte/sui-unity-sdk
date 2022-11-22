using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class UnityWebRequests
{
    public static IEnumerator Get(string uri, Action<UnityWebRequest> onSuccess = null, Action<UnityWebRequest> onError = null)
    {
        using var webRequest = UnityWebRequest.Get(uri);
        yield return webRequest.SendWebRequest();

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.Success:
                onSuccess?.Invoke(webRequest);
                break;
            default:
                Debug.LogError(": Error: " + webRequest.error);
                onError?.Invoke(webRequest);
                break;
        }
    }
    
    public static async Task<UnityWebRequest> GetAsync(string uri)
    {
        var webRequest = UnityWebRequest.Get(uri);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SendWebRequest();
        while (!webRequest.isDone)
        {
            await Task.Yield();
        }
        return webRequest;
    }
}
