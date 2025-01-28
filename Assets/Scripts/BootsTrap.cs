using UnityEngine;

public class BootsTrap : MonoBehaviour
{
    [SerializeField] private ScoreView score;
    [SerializeField] private GameField gameField;

    private PlayerData _playerData;

    private void Awake()
    {
        InitializeData();

        InitializeScore();

        InitializeField();
    }

    private void InitializeData()
    {
        _playerData = new PlayerData();
        //_playerData.ClearData();
    }

    private void InitializeScore()
    {
        score.Initialize(_playerData);
    }

    private void InitializeField()
    {
        gameField.Initialize(_playerData);
    }
}
