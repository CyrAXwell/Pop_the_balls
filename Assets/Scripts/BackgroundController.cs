using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject[] _backgrounds;
    [SerializeField] private GameObject[] _backgroundIcons;
    
    private int _currentBackground;

    private void Awake()
    {
        _currentBackground = PlayerPrefs.GetInt("background", 0);
        ChooseBackground();
    }

    private void ChooseBackground()
    {
        foreach (GameObject icons in _backgroundIcons)
        {
            icons.SetActive(false);
        }
        foreach (GameObject bg in _backgrounds)
        {
            bg.SetActive(false);
        }
        _backgrounds[_currentBackground].SetActive(true);
        _backgroundIcons[_currentBackground].SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_currentBackground == _backgrounds.Length - 1)
            _currentBackground = 0;
        else
            _currentBackground++;

        ChooseBackground();
    }
}
