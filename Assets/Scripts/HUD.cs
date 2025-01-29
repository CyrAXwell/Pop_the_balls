using System.Runtime.InteropServices;
using DG.Tweening;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ShowFullscreenAdv();

    private const bool Y_SDK_IS_ENABLED = YandexSDK.Y_SDK_IS_ENABLED;
    
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _OnSoundButton;
    [SerializeField] private GameObject _OffSoundButton;
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private float _fadeTime = 0.5f;
    [SerializeField] private CanvasGroup _transition;
    [SerializeField] private GameField _gameField;
    [SerializeField] private HighScoreView _highScoreView;
    [SerializeField] private CanvasGroup _losePanel;
    [SerializeField] private HighScoreView _losePanelHighScore;
    [SerializeField] private ScoreView _scoreView;

    private CanvasGroup _canvasGroupSettingsPanel => _settingsPanel.GetComponent<CanvasGroup>();

    private void Awake()
    {
        _settingsPanel.SetActive(false);
        _losePanel.gameObject.SetActive(false);
    }

    public void OpenSettingsPanel()
    {
        _settingsPanel.SetActive(true);
        _highScoreView.DisplayScore();
        _canvasGroupSettingsPanel.alpha = 0;
        _canvasGroupSettingsPanel.DOFade(1, _fadeTime);
    }

    public void CloseSettingsPanel()
    {
        _canvasGroupSettingsPanel.DOFade(0, _fadeTime).OnComplete(() => _settingsPanel.SetActive(false));
    }

    public void OnSound()
    {
        _audioManager.UnMuteSFX();
        _OnSoundButton.SetActive(false);
        _OffSoundButton.SetActive(true);
    }

    public void OffSound()
    {
        _audioManager.MuteSFX();
        _OnSoundButton.SetActive(true);
        _OffSoundButton.SetActive(false);
    }

    public void ResetTransition()
    {
#if UNITY_WEBGL && !UNITY_EDITOR && Y_SDK_IS_ENABLED
        ShowFullscreenAdv();
#endif
        _transition.gameObject.SetActive(true);
        _transition.alpha = 0;
        _transition.DOFade(1, _fadeTime).OnComplete(()=>
        {
            _gameField.ResetField();
            _transition.DOFade(0, _fadeTime).OnComplete(()=>
            {
                _transition.gameObject.SetActive(false);
                _gameField.AppearBalls();
            });
        });
    }

    public void OpenLosePanel()
    {
        _losePanel.gameObject.SetActive(true);
        _losePanelHighScore.DisplayScore();
        _scoreView.DisplayScoreOnLosePanel();
        _losePanel.alpha = 0;
        _losePanel.DOFade(1, _fadeTime);
    }

    public void OnContinueButton()
    {   
#if UNITY_WEBGL && !UNITY_EDITOR
        ShowFullscreenAdv();
#endif
        _losePanel.gameObject.SetActive(true);
        _gameField.ResetField();
        _losePanel.DOFade(0, _fadeTime).OnComplete(()=>
        {
            _losePanel.gameObject.SetActive(false);
            _gameField.AppearBalls();
        });
    }
}
