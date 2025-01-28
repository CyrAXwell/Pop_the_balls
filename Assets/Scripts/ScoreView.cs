using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreDisplay;
    [SerializeField] private TMP_Text losePanelScore;
    [SerializeField] private float countDuration;

    private float _current;
    private int _target;
    private PlayerData _playerData;
    private Coroutine _countScore;

    public void Initialize(PlayerData playerData)
    {
        //Debug.Log("Init Score ");
        _playerData = playerData;
        playerData.ScoreChanged += OnScoreChange;
        _current = 0;
        scoreDisplay.text = ((int)_current).ToString();
    }

    IEnumerator CountTo(int target)
    {
        //Debug.Log("CoutToTarget " + target);
        float rate = Mathf.Abs(target - _current) / countDuration;
        //Debug.Log("rate " + rate);
        while(_current != target)
        {
            _current = Mathf.MoveTowards(_current, target, rate * Time.deltaTime);
            scoreDisplay.text = ((int)_current).ToString();
            yield return null;
        }
    }

    public void OnScoreChange(int value)
    {
        if (value == 0)
        {
            _current = 0;
            scoreDisplay.text = ((int)_current).ToString();
        }
        else
        {
            _target = value;
            //Debug.Log("OnChangeScore " + _target);
            if (_countScore != null)
                StopCoroutine(_countScore);
            
            _countScore = StartCoroutine(CountTo(_target));
        }
    }

    public void DisplayScoreOnLosePanel()
    {
        losePanelScore.text = "Текущий счет: " + ((int)_target).ToString();
    }

    private void OnDestroy() => _playerData.ScoreChanged -= OnScoreChange;
}
