using TMPro;
using UnityEngine;

public class HighScoreView : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreDisplay;

    public void DisplayScore()
    {
        _scoreDisplay.text = "������: " + (PlayerPrefs.GetInt("Score", 0)).ToString();
    }
}
