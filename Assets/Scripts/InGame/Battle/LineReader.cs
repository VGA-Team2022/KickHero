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

    [Header("デバッグ時設定項目")]
    [Tooltip("単体テスト時true")]
    [SerializeField] bool _isDebug = false;
    [Tooltip("プレゼンター")]
    [SerializeField] BallPresenter _ballPresenter;

    LineRenderer _lineRenderer;
    List<(float time, Vector3 point)> _points = new List<(float, Vector3)>();
    bool _isDrawing = false;

    private void Start()
    {
        if (_isDebug)
        {
            Init();
        }
    }
    private void Update()
    {
        if (_isDebug)
        {
            OnUpdate(_ballPresenter);
        }
    }
    public void Init()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void OnUpdate(BallPresenter ballPresenter)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (StartEreaCheck(ballPresenter))
            {
                _points.Clear();
                _lineRenderer.positionCount = 0;
                if (ballPresenter)
                {
                    ballPresenter.Cancel();
                    ballPresenter.Collection();
                }
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (_isDrawing)
            {
                Vector3 position = Input.mousePosition;
                float time = _points.Count > 0 ? _points.LastOrDefault().time + Time.deltaTime : 0;
                position.z = 10;
                _points.Add((time, Camera.main.ScreenToWorldPoint(position)));
                _lineRenderer.positionCount = _points.Count;
                _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, Camera.main.ScreenToWorldPoint(position));
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            BallRoute route = RouteConvert(ballPresenter);
            if (route != null)
            {
                _lineRenderer.positionCount = route.Count;
                _lineRenderer.SetPositions(route.Positons);
                if (ballPresenter)
                {
                    if (ballPresenter.TryRouteSet(route))
                    {
                        ballPresenter.IsCollision = true;
                        ballPresenter.Shoot();
                        ballPresenter.OnCarryEnd(() => ballPresenter.IsCollision = false, false);
                    }
                }
            }
        }
    }

    bool StartEreaCheck(BallPresenter ballPresenter)
    {
        float x = Mathf.Abs(Input.mousePosition.x - Camera.main.WorldToScreenPoint(ballPresenter.StartPosition).x);
        float y = Mathf.Abs(Input.mousePosition.y - Camera.main.WorldToScreenPoint(ballPresenter.StartPosition).y);
        if (x <= _startErea.x / 2 && y <= _startErea.y / 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private BallRoute RouteConvert(BallPresenter ballPresenter)
    {
        if (!_enemy) { return null; }
        Vector3 eNomal = ballPresenter.StartPosition - _enemy.position;
        eNomal.y = 0;
        float h = Vector3.Dot(eNomal, _enemy.position);
        Vector3 dir = (_points.LastOrDefault().point - Camera.main.transform.position).normalized;
        Vector3 point = Camera.main.transform.position + (h - Vector3.Dot(eNomal, Camera.main.transform.position)) / Vector3.Dot(eNomal, dir) * dir;
        Vector3 normal = Vector3.Cross(point - _ballPresenter.StartPosition, Camera.main.transform.right);
        BallRoute route = new BallRoute();
        route.AddNode(ballPresenter.StartPosition, 0f);
        float buf = (Vector3.Dot(normal, ballPresenter.StartPosition) - Vector3.Dot(normal, Camera.main.transform.position));
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
        if(_startErea.x < 0)
        {
            _startErea.x = 0;
        }
        if(_startErea.y < 0)
        {
            _startErea.y = 0;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Vector2 center = Camera.main.WorldToScreenPoint(_ballPresenter.StartPosition); ;
        Vector2 erea = _startErea / 2;
        Vector3 rightTop = Camera.main.ScreenToWorldPoint(new(center.x + erea.x / 2, center.y + erea.y, 1));
        Vector3 rightBottom = Camera.main.ScreenToWorldPoint(new(center.x + erea.x / 2, center.y - erea.y, 1));
        //Debug.Log($"{Camera.main.ScreenToWorldPoint(_startErea / 2)}, {-Camera.main.ScreenToWorldPoint(_startErea / 2)}");
        Gizmos.DrawLine(rightTop, rightBottom);
    }

#endif

}
