using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameField : MonoBehaviour
{
    [SerializeField] private int _width = 16;
    [SerializeField] private int _hieght = 9;
    [SerializeField] private Cell _cellPrefab;
    [SerializeField] private int _spacing = 6;
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private HUD _hUD;

    private RectTransform _rt;
    private List<List<Cell>> _cells = new List<List<Cell>>();
    private bool _canPop = true;
    private Sequence _sequence;
    private int _startTween;
    private int _completeTween;
    private PlayerData _playerData;

    public void Initialize(PlayerData playerData)
    {
        _playerData = playerData;
        _rt = GetComponent<RectTransform>();
        CreateField();
    }

    private void OnDisable()
    {
        for (int i = 0; i < _cells.Count; i++)
        {
            for (int j = 0; j < _cells[0].Count; j++)
            {
                _cells[i][j].Click -= OnBallClick;
            }
        }
    }

    public void CreateField()
    {
        _canPop = true;
        float fieldWidth = _width * (_cellPrefab.Size + _spacing) + _spacing;
        float fieldHieght = _hieght * (_cellPrefab.Size + _spacing) + _spacing;

        _rt.sizeDelta = new Vector2(fieldWidth, fieldHieght);

        FillField(fieldWidth, fieldHieght);
    }

    private void FillField(float fieldWidth, float fieldHieght)
    {
        float StartX = -(fieldWidth / 2) + (_cellPrefab.Size / 2) + _spacing;
        float StartY = (fieldHieght / 2) - (_cellPrefab.Size / 2) - _spacing;

        for (int i = 0; i < _hieght; i++)
        {
            _cells.Add(new List<Cell>());

            for (int j = 0; j < _width; j++)
            {
                Cell cell = Instantiate(_cellPrefab, transform, false);
                cell.transform.localPosition = new Vector2(StartX + j * (_cellPrefab.Size + _spacing), StartY - i * (_cellPrefab.Size + _spacing));
                cell.Initialize(j, i);
                cell.Click += OnBallClick;
                _cells[i].Add(cell);
            }
        }
    }

    public void OnResetButton()
    {
        if (_canPop)
        {
            _canPop = false;
            _hUD.ResetTransition();
        }
    }

    public void ResetField()
    {
        _playerData.ResetScore();

        for (int i = 0; i < _hieght; i++)
        {
            for (int j = 0; j < _width; j++)
            {
                _cells[i][j].Reset();
            }
        }
    }

    public void AppearBalls()
    {
        for (int i = 0; i < _hieght; i++)
        {
            for (int j = 0; j < _width; j++)
            {
                StartCoroutine(SetPopWithDelay(0.9f));
                _cells[i][j].Appear(0.8f);
                _cells[i][j].DisplayBackground();
            }
        }
    }

    IEnumerator SetPopWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _canPop = true;
    }

    private void OnBallClick(Cell cell)
    {
        if (_canPop)
        {
            _canPop = false;
            List<Cell> BallsPack = new List<Cell>();
        
            CheckNeighbour(cell, BallsPack);

            _sequence = DOTween.Sequence();

            if (BallsPack.Count > 1)
            {
                _playerData.ChangeScore(BallsPack.Count * (BallsPack.Count - 1));

                foreach (Cell ball in BallsPack)
                {
                    _sequence.AppendCallback( () =>
                    {
                        ball.PopBall();
                        ball.PopEffect(_audioManager);
                    });
                    _sequence.AppendInterval(0.1f);
                }
                _sequence.AppendCallback( () =>
                {
                    ShiftDown();
                });
            }
            else
            {
                _canPop = true;
                cell.ClearPack();
                _audioManager.PlaySFX(_audioManager.PopSound);
            }
        }
    }

    private void CheckNeighbour(Cell cell, List<Cell> BallsPack)
    {
        cell.PutInPack();
        BallsPack.Add(cell);

        int x = cell.PosX;
        int y = cell.PosY;

        int right = x + 1;
        int left = x - 1;
        int up = y - 1;
        int down = y + 1;

        if (right < _cells[0].Count)
            CheckBall(_cells[y][right], BallsPack, cell.CellColor);
        
        if (left >= 0)
            CheckBall(_cells[y][left], BallsPack, cell.CellColor);
        
        if (up >= 0)
            CheckBall(_cells[up][x], BallsPack, cell.CellColor);
        
        if (down < _cells.Count)
            CheckBall(_cells[down][x], BallsPack, cell.CellColor);
            
    }

    private void CheckBall(Cell ball, List<Cell> BallsPack, CellColor target)
    {
        if (ball.CellColor == target && !ball.IsInPack)
            CheckNeighbour(ball, BallsPack);      
    }

    private void ShiftDown()
    {   
        _startTween = 0;
        _completeTween = 0;
        bool isDownMovement = false;
        for (int i = 0; i < _width; i++)
        {
            for (int j = _hieght - 1; j >= 0; j--)
            {  
                if (_cells[j][i].CellColor != CellColor.Empty && j < _hieght - 1 && _cells[j + 1][i].CellColor == CellColor.Empty)
                {
                    isDownMovement = true;
                    int k = j + 1;
                    while(k < _hieght && _cells[k][i].CellColor == CellColor.Empty)
                        k++;

                    _cells[k-1][i].SetColor(_cells[j][i].CellColor);
                    _cells[j][i].Move(_cells[k-1][i].GetComponent<Transform>(), _cells[k-1][i], true, this);
                    _cells[j][i].PopBall();
                }     
            }
        }
        if (!isDownMovement)
            ShiftLeft();
    }
    
    public void StartTween()
    {
        _startTween++;
    }

    public void CompleteShiftDown()
    {
        _completeTween++;
        if(_completeTween == _startTween)
        {
            ShiftLeft();
        }
    }

    public void CompleteShiftLeft()
    {
        _completeTween++;
        if(_completeTween == _startTween)
        {
            AddNewLines();
        }
    }

    public void CompleteAddLines()
    {
        _completeTween++;
        if(_completeTween == _startTween)
        {
            OnAnimationsComplete();
        }
    }

    private void OnAnimationsComplete()
    {
        _canPop = true;
        CheckMathces();
    }

    private void ShiftLeft()
    {
        _startTween = 0;
        _completeTween = 0;
        bool isLeftMovement = false;
        for (int i = 0; i < _width; i++)
        {
            if (i > 0 && _cells[_hieght - 1][i].CellColor != CellColor.Empty && _cells[_hieght - 1][i - 1].CellColor == CellColor.Empty)
            {
                isLeftMovement = true;
                int k = i - 1;
                while(k >= 0 && _cells[_hieght - 1][k].CellColor == CellColor.Empty)
                    k--;

                for (int j = 0; j < _hieght; j++)
                {
                    if (_cells[j][i].CellColor != CellColor.Empty && _cells[j][k+1].CellColor == CellColor.Empty)
                    {
                        _cells[j][k+1].SetColor(_cells[j][i].CellColor);
                        _cells[j][i].Move(_cells[j][k+1].GetComponent<Transform>(), _cells[j][k+1], false, this);
                        _cells[j][i].PopBall();
                    }
                }
            }
        }
        if (!isLeftMovement)
            AddNewLines();
    }

    private void AddNewLines()
    {
        bool isAddNewLines = false;
        _startTween = 0;
        _completeTween = 0;
        for (int i = 0; i < _width; i++)
        {
            if(i > 0 && _cells[_hieght - 1][i].CellColor == CellColor.Empty && _cells[_hieght - 1][i - 1].CellColor != CellColor.Empty)
            {
                isAddNewLines = true;
                for (int j = 0; j < _hieght; j++)
                {
                    _cells[j][i].Appear(this, 0.5f);
                }
            }
        }
        if (!isAddNewLines)
            OnAnimationsComplete();
    }

    private void CheckMathces()
    {
        bool isLose = true;

        for (int i = 0; i < _hieght; i++)
        {
            if (!isLose)
                break;
            
            for (int j = 0; j < _width; j++)
            {
                int left = j - 1;
                int up = i - 1;
                int down = i + 1;

                if ((j + 1 < _cells[0].Count && IsEqualBalls(_cells[i][j], _cells[i][j + 1])) ||
                (j - 1 >= 0 && IsEqualBalls(_cells[i][j], _cells[i][j - 1])) ||
                (i - 1 >= 0 && IsEqualBalls(_cells[i][j], _cells[i - 1][j])) ||
                (i + 1 < _cells.Count && IsEqualBalls(_cells[i][j], _cells[i + 1][j])))
                {
                    isLose = false;
                    break;
                }
            }
        }

        if (isLose)
        {
            _canPop = false;
            _hUD.OpenLosePanel();
        }
    }

    private bool IsEqualBalls(Cell ball1, Cell ball2)
    {
        return ball1.CellColor != CellColor.Empty && ball2.CellColor != CellColor.Empty && ball1.CellColor == ball2.CellColor;
    }
}
