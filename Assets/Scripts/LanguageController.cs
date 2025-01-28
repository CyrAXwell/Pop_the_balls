using UnityEngine;
using UnityEngine.UI;

public class LanguageController : MonoBehaviour
{
    [SerializeField] private Button enButton;
    [SerializeField] private Button ruButton;
    [SerializeField] private Color enableColor;
    [SerializeField] private Color disableColor;

    private enum languages
    {
        en,
        ru
    }


}
