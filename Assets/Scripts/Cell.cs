using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell : MonoBehaviour, IPointerClickHandler
{
    public event System.Action<Cell> Click;
    
    [SerializeField] private int size;
    [SerializeField] private Image background;
    [SerializeField] private GameObject backgroundPref;
    [SerializeField] private GameObject redBall;
    [SerializeField] private GameObject blueBall;
    [SerializeField] private GameObject greenBall;
    [SerializeField] private GameObject yellowBall;
    [SerializeField] private GameObject purpleBall;

    [SerializeField] private GameObject redPopEffect;
    [SerializeField] private GameObject bluePopEffect;
    [SerializeField] private GameObject greenPopEffect;
    [SerializeField] private GameObject yellowPopEffect;
    [SerializeField] private GameObject purplePopEffect;

    public int PosX { get; private set; }
    public int PosY { get; private set; }   
    public CellColor CellColor { get; private set; } 
    public bool IsInPack { get; private set; }
    
    public int Size => size;

    private Color _backgroundColor;
    private GameObject _ball;
    private GameObject _popEffect;
    
    

    public void Initialize(int posX, int posY)
    {
        PosX = posX;
        PosY = posY;
        CellColor = PutBall();
        DisplayColor();
        _backgroundColor = background.color; 
    }

    public void Reset()
    {
        if (_ball != null)
        {
            Color temp = background.color;
            temp.a = 0;
            background.color = temp;
            Destroy(_ball);
        }

        
        //Appear();
        // CellColor = PutBall();
        // DisplayColor();
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
                _ball = Instantiate(redBall, transform, false);
                _popEffect = redPopEffect;
                break;
            case CellColor.Blue:
                _ball = Instantiate(blueBall, transform, false);
                _popEffect = bluePopEffect;
                break;
            case CellColor.Green:
                _ball = Instantiate(greenBall, transform, false);
                _popEffect = greenPopEffect;
                break;
            case CellColor.Yellow:
                _ball = Instantiate(yellowBall, transform, false);
                _popEffect = yellowPopEffect;
                break;
            case CellColor.Purple:
                _ball = Instantiate(purpleBall, transform, false);
                _popEffect = purplePopEffect;
                break;
        }
    }

    public void DisplayBackground()
    {
        background.color = _backgroundColor;
    }

    public void SetColor(CellColor color)
    {
        CellColor = color;
        //background.color = _backgroundColor;
        //DisplayColor();
    }

    public void SetRandomColor()
    {
        CellColor = PutBall();
        background.color = _backgroundColor;
        //DisplayColor();
    }

    public void Move(Transform target, Cell to, bool isMoveDown, GameField controller)
    {
        GameObject ball = SpawnBall();
        var rt = ball.GetComponent<RectTransform>();

        GameObject cellBackground = Instantiate(backgroundPref, transform, false);
        //Debug.Log(ball);
        
        float duration;
        if (isMoveDown)
            duration = (gameObject.transform.localPosition.y - target.localPosition.y) / (size * 5f);
        else
            duration = (gameObject.transform.localPosition.x - target.localPosition.x) / (size * 3f);
        
        //ball.transform.parent = target;
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
        //target.localPosition.y - gameObject.transform.localPosition.y
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
                return Instantiate(redBall, transform, false);
            case CellColor.Blue:
                return Instantiate(blueBall, transform, false);
            case CellColor.Green:
                return Instantiate(greenBall, transform, false);
            case CellColor.Yellow:
                return Instantiate(yellowBall, transform, false);
            case CellColor.Purple:
                return Instantiate(purpleBall, transform, false);
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

        Color temp = background.color;
        temp.a = 0;
        background.color = temp;  
    }

    public void PopEffect(AudioManager audioManager)
    {
        audioManager.PlaySFX(audioManager.PopSound);
        Instantiate(_popEffect, transform, false);
    }

    public void PutInPack() => IsInPack = true;

    public void ClearPack() => IsInPack = false;
}
