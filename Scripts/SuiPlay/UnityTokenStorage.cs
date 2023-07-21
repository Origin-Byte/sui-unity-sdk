using Suinet.SuiPlay;
using UnityEngine;

public class UnityTokenStorage : ITokenStorage
{
    public void SaveToken(string token)
    {
        PlayerPrefs.SetString("accessToken", token);
        PlayerPrefs.Save();
    }

    public string LoadToken()
    {
        return PlayerPrefs.HasKey("accessToken") ? PlayerPrefs.GetString("accessToken") : null;
    }
}