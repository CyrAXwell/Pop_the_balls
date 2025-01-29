using UnityEngine;

public class BootsTrap : MonoBehaviour
{
    [SerializeField] private ScoreView _score;
    [SerializeField] private GameField _gameField;

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
    }

    private void InitializeScore()
    {
        _score.Initialize(_playerData);
    }

    private void InitializeField()
    {
        _gameField.Initialize(_playerData);
    }
}
