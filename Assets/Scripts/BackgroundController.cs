using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundController : MonoBehaviour, IPointerClickHandler
{
    // [SerializeField] private GameObject solidBackground;
    // [SerializeField] private GameObject heartBackground;
    // [SerializeField] private GameObject starBackground;
    // [SerializeField] private GameObject squareBackground;
    // [SerializeField] private GameObject pentagonBackground;
    // [SerializeField] private GameObject rombBackground;

    [SerializeField] private GameObject[] backgrounds;
    [SerializeField] private GameObject[] backgroundIcons;
    
    private int _currentBackground;

    private void Awake()
    {
        _currentBackground = PlayerPrefs.GetInt("background", 0);
        ChooseBackground();
    }

    private void ChooseBackground()
    {
        foreach (GameObject icons in backgroundIcons)
        {
            icons.SetActive(false);
        }
        foreach (GameObject bg in backgrounds)
        {
            bg.SetActive(false);
        }
        backgrounds[_currentBackground].SetActive(true);
        backgroundIcons[_currentBackground].SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_currentBackground == backgrounds.Length - 1)
            _currentBackground = 0;
        else
            _currentBackground++;

        ChooseBackground();
    }



}
