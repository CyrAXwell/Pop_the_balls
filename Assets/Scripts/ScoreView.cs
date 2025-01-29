using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreDisplay;
    [SerializeField] private TMP_Text _losePanelScore;
    [SerializeField] private float countDuration = 1;

    private float _current;
    private int _target;
    private PlayerData _playerData;
    private Coroutine _countScore;

    public void Initialize(PlayerData playerData)
    {
        _playerData = playerData;
        playerData.ScoreChanged += OnScoreChange;
        _current = 0;
        _scoreDisplay.text = ((int)_current).ToString();
    }

    IEnumerator CountTo(int target)
    {
        float rate = Mathf.Abs(target - _current) / countDuration;

        while(_current != target)
        {
            _current = Mathf.MoveTowards(_current, target, rate * Time.deltaTime);
            _scoreDisplay.text = ((int)_current).ToString();
            yield return null;
        }
    }

    public void OnScoreChange(int value)
    {
        if (value == 0)
        {
            _current = 0;
            _scoreDisplay.text = ((int)_current).ToString();
        }
        else
        {
            _target = value;
            if (_countScore != null)
                StopCoroutine(_countScore);
            
            _countScore = StartCoroutine(CountTo(_target));
        }
    }

    public void DisplayScoreOnLosePanel()
    {
        _losePanelScore.text = "Текущий счет: " + ((int)_target).ToString();
    }

    private void OnDestroy() => _playerData.ScoreChanged -= OnScoreChange;
}
