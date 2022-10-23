using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

/// <summary>
/// ボールの制御クラス
/// アルファ版
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class BallAlpha : MonoBehaviour
{
    [Tooltip("ボールの跳び方")]
    [SerializeField] KickType _kickType;
    [Tooltip("ボールのスピード")]
    [SerializeField] float _speed = 10;
    [SerializeField] float _curve = 1;
    [Header("========これより下は触らない====================")]
    [SerializeField] GameObject _ball;
    [SerializeField] GameObject _ball2;
    [SerializeField] GameObject _plane;
    List<Vector3> _points = new List<Vector3>();
    LineRenderer _lineRenderer;
    Vector3[] _route;
    bool _isMove = false;
    Vector3 _forward;
    Vector3 _up;
    Vector3 _startPoint;
    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _startPoint = _ball.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isMove)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _ball.transform.position = _startPoint;
                _points.Clear();
                _lineRenderer.positionCount = 0;
                _plane.transform.position = new Vector3(0, -200, 0);
            }
            if (Input.GetMouseButton(0))
            {
                Vector3 position = Input.mousePosition;
                _points.Add(position);
                position.z = 10;
                _lineRenderer.positionCount = _points.Count;
                _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, Camera.main.ScreenToWorldPoint(position));
            }
            if (Input.GetMouseButtonUp(0))
            {
                StartCoroutine(Point());
                _lineRenderer.positionCount = 0;
            }
        }
        Debug.DrawRay(_startPoint, _forward * 100);
        Debug.DrawRay(_ball2.transform.position, _up * -100);
    }

    IEnumerator Point()
    {
        _route = new Vector3[_points.Count];

        Ray ray = Camera.main.ScreenPointToRay(_points.Last());
        if (Physics.Raycast(ray, out RaycastHit hit, 100))
        {
            _route[_route.Length - 1] = hit.point;
            _plane.transform.position = Vector3.Lerp(_ball.transform.position, hit.point, 0.5f);
            float rotX = -Mathf.Atan2(hit.point.y - _ball.transform.position.y, hit.point.z - _ball.transform.position.z) / Mathf.PI * 180;
            _plane.transform.eulerAngles = new Vector3(rotX, Camera.main.transform.eulerAngles.y, 0);

            yield return new WaitForFixedUpdate();
            for (int i = 0; i < _points.Count - 1; i++)
            {
                ray = Camera.main.ScreenPointToRay(_points[i]);
                if (Physics.Raycast(ray, out hit, 100))
                {
                    _route[i] = hit.point;
                    Debug.DrawLine(ray.origin, hit.point);
                }
            }
            UnityEditor.EditorApplication.isPaused = true;
            if (_kickType == KickType.Curve)
            {
                Vector3 goal = _route.Last();
                Vector3 relayPoint = new Vector3(_route.Average(v => v.x), _route.Average(v => v.y), _route.Average(v => v.z));
                _ball2.transform.position = relayPoint;
                float length = Vector3.Distance(_startPoint, relayPoint) + Vector3.Distance(relayPoint, goal);
                float num = length / _speed;
                _route = new Vector3[Mathf.CeilToInt(num)];
                Vector3 sr = relayPoint - _startPoint;
                Vector3 rg = goal - relayPoint;
                _forward = (goal - _startPoint).normalized;
                _up = Vector3.Cross(Vector3.Cross(rg, sr), _forward).normalized;
                for (int k = 0; k < _route.Length; k++)
                {
                    float t = Mathf.Abs(Mathf.Cos(((k - _route.Length / 2f) / _route.Length) * Mathf.PI));
                    _route[k] = Vector3.Lerp(_startPoint, goal, (1f / _route.Length) * (k + 1)) + _up * t * Vector3.Dot(_up, relayPoint - _startPoint) * _curve;
                    Debug.Log(t);
                }
            }
            StartCoroutine(Move());
        }
    }

    IEnumerator Move()
    {
        _isMove = true;
        for (int i = 0; i < _route.Length; i++)
        {
            _ball.transform.position = _route[i];
            yield return null;
        }
        _isMove = false;
    }
}


enum KickType
{
    Straight,
    Curve,
    Technical,
}