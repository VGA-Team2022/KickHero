using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class LineReader : MonoBehaviour
{
    [Tooltip("ボスのTransform")]
    [SerializeField] Transform _enemy;
    [Tooltip("入力開始可能領域")]
    [SerializeField] Vector2 _startErea;
    [Tooltip("線を引ける時間")]
    [SerializeField] float _drawTime = 1f;

    [Header("デバッグ時設定項目")]
    [Tooltip("単体テスト時true")]
    [SerializeField] bool _isDebug = false;
    [Tooltip("プレゼンター")]
    [SerializeField] BallPresenter _ballPresenter;


    LineRenderer _lineRenderer;
    List<(float time, Vector3 point)> _points = new List<(float, Vector3)>();
    bool _isDrawing = false;
    float _front = 2.0f;
    Color _gizmosColor = Color.red;
    float _time = 0;
    Action _onDrawStartAction;
    Action _onDrawEndAction;

    public BallPresenter BallPresenter { get => _ballPresenter; set => _ballPresenter = value; }

    /// <summary>残り時間の割合</summary>
    public float TimeGage { get => (_drawTime - _time) / _drawTime; }

    /// <summary>今線を引いているか</summary>
    public bool IsDrawing { get => _isDrawing; }

    private void Start()
    {
        if (_isDebug)
        {
            Init();
        }
    }
    private void Update()
    {
        if (_ballPresenter && _isDebug)
        {
            OnUpdate();
        }
    }
    public void Init()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void Init(BallPresenter ballPresenter)
    {
        _ballPresenter = ballPresenter;
        Init();
    }

    public void OnUpdate()
    {
        if (!_ballPresenter) { return; }
        if (Input.GetMouseButtonDown(0))
        {
            if (StartEreaCheck())
            {
                _time = 0;
                _isDrawing = true;
                _points.Clear();
                _lineRenderer.positionCount = 0;
                if (_ballPresenter)
                {
                    _ballPresenter.Cancel();
                    _ballPresenter.Collection();
                }
                RecordPoint();
                CallOnDrawStart();
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (_isDrawing)
            {
                _time += Time.deltaTime;
                if(_time < _drawTime)
                {
                    RecordPoint();
                }
                else
                {
                    DrawFinish();
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            DrawFinish();
        }
    }

    public void OnUpdate(BallPresenter ballPresenter)
    {
        _ballPresenter = ballPresenter;
        OnUpdate();
    }

    public void OnDrawStart(Action action)
    {
        _onDrawStartAction += action;
    }

    public void OnDrawEnd(Action action)
    {
        _onDrawEndAction += action;
    }

    void CallOnDrawStart()
    {
        if(_onDrawStartAction != null)
        {
            _onDrawStartAction.Invoke();
        }
    }

    void CallOnDrawEnd()
    {
        if(_onDrawEndAction != null)
        {
            _onDrawEndAction.Invoke();
        }
    }

    bool StartEreaCheck()
    {
        if (!_ballPresenter) { return false; }
        float x = Mathf.Abs(Input.mousePosition.x - Camera.main.WorldToScreenPoint(_ballPresenter.StartPosition).x);
        float y = Mathf.Abs(Input.mousePosition.y - Camera.main.WorldToScreenPoint(_ballPresenter.StartPosition).y);
        if (x <= _startErea.x / 2 && y <= _startErea.y / 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void RecordPoint()
    {
        Vector3 position = Input.mousePosition;
        float time = _points.Count > 0 ? _points.LastOrDefault().time + Time.deltaTime : 0;
        position.z = 10;
        _points.Add((time, Camera.main.ScreenToWorldPoint(position)));
        _lineRenderer.positionCount = _points.Count;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, Camera.main.ScreenToWorldPoint(position));
    }

    void DrawFinish()
    {
        _isDrawing = false;
        BallRoute route = RouteConvert();
        if (route != null)
        {
            _lineRenderer.positionCount = route.Count;
            _lineRenderer.SetPositions(route.Positons);
            if (_ballPresenter)
            {
                if (_ballPresenter.TryRouteSet(route))
                {
                    _ballPresenter.IsCollision = true;
                    _ballPresenter.Shoot();
                    //_ballPresenter.OnCarryEnd(() => _ballPresenter.IsCollision = false, false);
                }
            }
        }
        CallOnDrawEnd();
    }

    private BallRoute RouteConvert()
    {
        if (!_ballPresenter && !_enemy) { return null; }
        Vector3 eNomal = _ballPresenter.StartPosition - _enemy.position;
        eNomal.y = 0;
        float h = Vector3.Dot(eNomal, _enemy.position);
        Vector3 dir = (_points.LastOrDefault().point - Camera.main.transform.position).normalized;
        Vector3 point = Camera.main.transform.position + (h - Vector3.Dot(eNomal, Camera.main.transform.position)) / Vector3.Dot(eNomal, dir) * dir;
        Vector3 normal = Vector3.Cross(point - _ballPresenter.StartPosition, Camera.main.transform.right);
        BallRoute route = new BallRoute();
        route.AddNode(_ballPresenter.StartPosition, 0f);
        float buf = (Vector3.Dot(normal, _ballPresenter.StartPosition) - Vector3.Dot(normal, Camera.main.transform.position));
        for (int i = 0; i < _points.Count; i++)
        {
            dir = (_points[i].point - Camera.main.transform.position).normalized;
            point = Camera.main.transform.position + buf / Vector3.Dot(normal, dir) * dir;
            route.AddNode(point, _points[i].time);
        }
        return route;
    }



#if UNITY_EDITOR

    private void OnValidate()
    {
        if (_startErea.x < 0)
        {
            _startErea.x = 0;
        }
        if (_startErea.y < 0)
        {
            _startErea.y = 0;
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (!_ballPresenter) { return; }
        Vector2 center = Camera.main.WorldToScreenPoint(_ballPresenter.StartPosition); ;
        Vector2 erea = _startErea / 2;
        Vector3 rightTop = Camera.main.ScreenToWorldPoint(new(center.x + erea.x, center.y + erea.y, _front));
        Vector3 rightBottom = Camera.main.ScreenToWorldPoint(new(center.x + erea.x, center.y - erea.y, _front));
        Vector3 leftTop = Camera.main.ScreenToWorldPoint(new(center.x - erea.x, center.y + erea.y, _front));
        Vector3 leftBottom = Camera.main.ScreenToWorldPoint(new(center.x - erea.x, center.y - erea.y, _front));

        Gizmos.color = _gizmosColor;

        Gizmos.DrawLine(rightTop, rightBottom);     //右
        Gizmos.DrawLine(leftTop, leftBottom);       //左
        Gizmos.DrawLine(leftTop, rightTop);         //上
        Gizmos.DrawLine(rightBottom, leftBottom);   //下
    }

#endif

}
