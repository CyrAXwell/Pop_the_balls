using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell : MonoBehaviour, IPointerClickHandler
{
    public event System.Action<Cell> Click;
    
    [SerializeField] private int _size = 80;
    [SerializeField] private Image _background;
    [SerializeField] private GameObject _backgroundPref;
    [SerializeField] private GameObject _redBall;
    [SerializeField] private GameObject _blueBall;
    [SerializeField] private GameObject _greenBall;
    [SerializeField] private GameObject _yellowBall;
    [SerializeField] private GameObject _purpleBall;
    [SerializeField] private GameObject _redPopEffect;
    [SerializeField] private GameObject _bluePopEffect;
    [SerializeField] private GameObject _greenPopEffect;
    [SerializeField] private GameObject _yellowPopEffect;
    [SerializeField] private GameObject _purplePopEffect;

    private Color _backgroundColor;
    private GameObject _ball;
    private GameObject _popEffect;

    public int PosX { get; private set; }
    public int PosY { get; private set; }   
    public CellColor CellColor { get; private set; } 
    public bool IsInPack { get; private set; }
    public int Size => _size;

    public void Initialize(int posX, int posY)
    {
        PosX = posX;
        PosY = posY;
        CellColor = PutBall();
        DisplayColor();
        _backgroundColor = _background.color; 
    }

    public void Reset()
    {
        if (_ball != null)
        {
            Color temp = _background.color;
            temp.a = 0;
            _background.color = temp;
            Destroy(_ball);
        }
    }

    private CellColor PutBall()
    {
        return (CellColor) Random.Range(1, 6);
    }

    public void DisplayColor()
    {
        switch (CellColor)
        {
            case CellColor.Red:
                _ball = Instantiate(_redBall, transform, false);
                _popEffect = _redPopEffect;
                break;
            case CellColor.Blue:
                _ball = Instantiate(_blueBall, transform, false);
                _popEffect = _bluePopEffect;
                break;
            case CellColor.Green:
                _ball = Instantiate(_greenBall, transform, false);
                _popEffect = _greenPopEffect;
                break;
            case CellColor.Yellow:
                _ball = Instantiate(_yellowBall, transform, false);
                _popEffect = _yellowPopEffect;
                break;
            case CellColor.Purple:
                _ball = Instantiate(_purpleBall, transform, false);
                _popEffect = _purplePopEffect;
                break;
        }
    }

    public void DisplayBackground()
    {
        _background.color = _backgroundColor;
    }

    public void SetColor(CellColor color)
    {
        CellColor = color;
    }

    public void SetRandomColor()
    {
        CellColor = PutBall();
        _background.color = _backgroundColor;
    }

    public void Move(Transform target, Cell to, bool isMoveDown, GameField controller)
    {
        GameObject ball = SpawnBall();
        var rt = ball.GetComponent<RectTransform>();

        GameObject cellBackground = Instantiate(_backgroundPref, transform, false);
        
        float duration;
        if (isMoveDown)
            duration = (gameObject.transform.localPosition.y - target.localPosition.y) / (_size * 5f);
        else
            duration = (gameObject.transform.localPosition.x - target.localPosition.x) / (_size * 3f);
        
        cellBackground.transform.SetParent(target);
        ball.transform.SetParent(cellBackground.transform);

        cellBackground.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), duration);

        rt.DOAnchorPos(new Vector2(0, 0), duration).OnComplete(() => 
        {
            Destroy(ball);
            Destroy(cellBackground);
            to.DisplayColor();
            to.DisplayBackground();
            if (isMoveDown)
                controller.CompleteShiftDown();
            else    
                controller.CompleteShiftLeft();
        }).OnStart(() => controller.StartTween());
    }

    public void Appear(GameField controller, float time)
    {
        SetRandomColor();
        GameObject ball = SpawnBall();

        controller.StartTween();

        ball.transform.localScale = new Vector3(0, 0, 0);
        ball.transform.DOScale(new Vector3(1, 1, 1), time).OnComplete(()=>
        {
            controller.CompleteAddLines();
            Destroy(ball);
            DisplayColor();
        });
    }

    public void Appear(float time)
    {
        SetRandomColor();
        GameObject ball = SpawnBall();

        ball.transform.localScale = new Vector3(0, 0, 0);
        ball.transform.DOScale(new Vector3(1, 1, 1), time).OnComplete(()=>
        {
            Destroy(ball);
            DisplayColor();
        });
    }

    private GameObject SpawnBall()
    {
        switch (CellColor)
        {
            case CellColor.Red:
                return Instantiate(_redBall, transform, false);
            case CellColor.Blue:
                return Instantiate(_blueBall, transform, false);
            case CellColor.Green:
                return Instantiate(_greenBall, transform, false);
            case CellColor.Yellow:
                return Instantiate(_yellowBall, transform, false);
            case CellColor.Purple:
                return Instantiate(_purpleBall, transform, false);
            default:
                return null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (CellColor != CellColor.Empty)
            Click?.Invoke(this);
    }

    public void PopBall()
    {
        IsInPack = false;
        if(_ball != null)
            Destroy(_ball);
        
        CellColor = CellColor.Empty;

        Color temp = _background.color;
        temp.a = 0;
        _background.color = temp;  
    }

    public void PopEffect(AudioManager audioManager)
    {
        audioManager.PlaySFX(audioManager.PopSound);
        Instantiate(_popEffect, transform, false);
    }

    public void PutInPack() => IsInPack = true;

    public void ClearPack() => IsInPack = false;
}
