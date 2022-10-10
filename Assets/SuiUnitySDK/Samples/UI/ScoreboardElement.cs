using TMPro;
using UnityEngine;

public class ScoreboardElement : MonoBehaviour
{
    public TMP_Text playerNameText;
    public TMP_Text scoreText;

    public void UpdateElementData(string playerName, ulong score)
    {
        playerNameText.SetText(playerName);
        scoreText.SetText(score.ToString());
    }
}
