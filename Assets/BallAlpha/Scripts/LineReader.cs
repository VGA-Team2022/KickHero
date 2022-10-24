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
    [SerializeField] BallMove _ball;

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
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 position = Input.mousePosition;
            float time = _points.Count > 0 ? _points.LastOrDefault().time + Time.deltaTime : 0;
            position.z = 10;
            _points.Add((time, position));
            _lineRenderer.positionCount = _points.Count;
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, Camera.main.ScreenToWorldPoint(position));
        }
        if (Input.GetMouseButtonUp(0))
        {
            RouteConvert(_points.LastOrDefault().point);
        }
    }

    private void RouteConvert(Vector3 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, 100))
        {
            float rotX = -Mathf.Atan2(_start.transform.position.y - hit.point.y, _start.transform.position.z - hit.point.z) / Mathf.PI * 180;
            Vector3 normal = new Vector3(rotX, Camera.main.transform.eulerAngles.y, 0);
            BallRoute route = new BallRoute();
            route.AddNode(_start.position, 0f);
            for (int i = 0; i < _points.Count - 1; i++)
            {
                Vector3 dir = (_points[i].point - Camera.main.transform.position ).normalized;
                Vector3 point = Camera.main.transform.position + (Vector3.Dot(normal, _start.position) - Vector3.Dot(normal, Camera.main.transform.position)) / Vector3.Dot(normal, dir) * dir;
                route.AddNode(point, _points[i].time);
            }
            _ball.Shoot(route);
        }
    }
}
