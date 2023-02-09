using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;
using Suinet.Faucet;
using UnityEngine;
using UnityEngine.Networking;

public class UnityWebRequestFaucetClient : IFaucetClient
{
    public Uri Endpoint { get; private set; }

    public UnityWebRequestFaucetClient(string url = "https://faucet.devnet.sui.io/gas")
    {
        Endpoint = new Uri(url);
    }

    public async Task<bool> AirdropGasAsync(string recipient)
    {
        using (var unityWebRequest = new UnityWebRequest(Endpoint, "POST"))
        {
            var airdropRequest = new AirdropRequest(recipient);
            var json = JsonConvert.SerializeObject(airdropRequest);
            var requestData = Encoding.UTF8.GetBytes(json);
            
            unityWebRequest.uploadHandler = new UploadHandlerRaw(requestData);
            unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
            unityWebRequest.SetRequestHeader("Content-Type", "application/json");

            unityWebRequest.SendWebRequest();
            while (!unityWebRequest.isDone)
            {
                await Task.Yield();
            }

            if (unityWebRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(unityWebRequest.error);
            }

            return unityWebRequest.result == UnityWebRequest.Result.Success;
        }
    }
}
