using UnityEngine;
using UnityEngine.UI;

public class LanguageController : MonoBehaviour
{
    [SerializeField] private Button _enButton;
    [SerializeField] private Button _ruButton;
    [SerializeField] private Color _enableColor;
    [SerializeField] private Color _disableColor;

    private enum languages
    {
        en,
        ru
    }
}
