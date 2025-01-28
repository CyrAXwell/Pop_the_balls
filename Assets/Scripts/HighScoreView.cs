using TMPro;
using UnityEngine;

public class HighScoreView : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreDisplay;

    public void DisplayScore()
    {
        scoreDisplay.text = "������: " + (PlayerPrefs.GetInt("Score", 0)).ToString();
    }
}
