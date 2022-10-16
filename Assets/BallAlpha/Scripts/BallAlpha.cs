using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ボールの制御クラス
/// アルファ版
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class BallAlpha : MonoBehaviour
{
    [Header("========これより下は触らない====================")]
    [SerializeField] GameObject _ball;
    [SerializeField] GameObject _plane;
    List<Vector3> _points = new List<Vector3>();
    LineRenderer _lineRenderer;
    Vector3[] _route;
    bool _isMove = false;
    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isMove)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _points.Clear();
                _lineRenderer.positionCount = 0;
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
    }

    IEnumerator Point()
    {
        _route = new Vector3[_points.Count];

        Ray ray = Camera.main.ScreenPointToRay(_points.Last());
        if (Physics.Raycast(ray, out RaycastHit hit, 100))
        {
            _route[_route.Length - 1] = hit.point;
            _plane.transform.position = Vector3.Lerp(_ball.transform.position, hit.point, 0.5f);
            float rotX = Mathf.Atan2(hit.point.y - _ball.transform.position.y, hit.point.z - _ball.transform.position.z);
            _plane.transform.eulerAngles = new Vector3(-rotX / Mathf.PI * 180, _plane.transform.eulerAngles.y, _plane.transform.eulerAngles.z);

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
            StartCoroutine(Move());
        }
    }

    IEnumerator Move()
    {
        _isMove = true;
        for(int i = 0; i < _route.Length; i++)
        {
            _ball.transform.position = _route[i];
            yield return null;
        }
        _isMove = false;    
    }
}
