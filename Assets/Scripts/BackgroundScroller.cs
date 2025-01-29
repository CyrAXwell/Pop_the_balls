using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private RawImage _image;
    [SerializeField, Range(0, 10)] private float _speed = 0.1f;
    [SerializeField, Range(-1, 1)] private float _xDirection = 1;
    [SerializeField, Range(-1, 1)] private float y_Direction = 1;

    private void Update()
    {
        _image.uvRect = new Rect(_image.uvRect.position + new Vector2(- _xDirection * _speed, y_Direction * _speed) * Time.deltaTime, _image. uvRect.size);
    }
}
