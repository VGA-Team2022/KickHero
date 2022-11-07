using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(LineRenderer))]
public class LineReader : MonoBehaviour
{
    [SerializeField] Transform _start;
    [SerializeField] Transform _enemy;
    //[SerializeField] BallMove _ball;
    [SerializeField] BallPresenter _ballPresenter;

    LineRenderer _lineRenderer;
    List<(float time, Vector3 point)> _points = new List<(float, Vector3)>();

    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _points.Clear();
            _lineRenderer.positionCount = 0;
            if (_ballPresenter)
            {
                _ballPresenter.BallModel.Cancel();
                _ballPresenter.BallModel.Collection();
            }
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 position = Input.mousePosition;
            float time = _points.Count > 0 ? _points.LastOrDefault().time + Time.deltaTime : 0;
            position.z = 10;
            _points.Add((time, Camera.main.ScreenToWorldPoint(position)));
            _lineRenderer.positionCount = _points.Count;
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, Camera.main.ScreenToWorldPoint(position));
        }
        if (Input.GetMouseButtonUp(0))
        {
            BallRoute route = RouteConvert(Camera.main.WorldToScreenPoint(_points.LastOrDefault().point));
            if (route != null)
            {
                _lineRenderer.positionCount = route.Count;
                _lineRenderer.SetPositions(route.Positons);
                if (_ballPresenter)
                {
                    BallModel ballModel = _ballPresenter.BallModel;
                    if (_ballPresenter.BallModel.TryRouteSet(route))
                    {
                        _ballPresenter.BallModel.Shoot();
                    }
                }
            }
        }
    }

    private BallRoute RouteConvert(Vector3 position)
    {
        if(!_start || !_enemy) { return null; }
        Vector3 eNomal = _start.position - _enemy.position;
        eNomal.y = 0;
        float h = Vector3.Dot(eNomal, _enemy.position);
        Vector3 dir = (_points.LastOrDefault().point - Camera.main.transform.position).normalized;
        Vector3 point = Camera.main.transform.position + (h - Vector3.Dot(eNomal, Camera.main.transform.position)) / Vector3.Dot(eNomal, dir) * dir;
        Vector3 normal = Vector3.Cross(point - _start.transform.position, Camera.main.transform.right);
        BallRoute route = new BallRoute();
        route.AddNode(_start.position, 0f);
        float buf = (Vector3.Dot(normal, _start.position) - Vector3.Dot(normal, Camera.main.transform.position));
        for (int i = 0; i < _points.Count; i++)
        {
            dir = (_points[i].point - Camera.main.transform.position).normalized;
            point = Camera.main.transform.position + buf / Vector3.Dot(normal, dir) * dir;
            route.AddNode(point, _points[i].time);
        }
        return route;
    }

}
