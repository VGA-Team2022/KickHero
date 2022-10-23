using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineReader : MonoBehaviour
{
    [SerializeField] Transform _start;
    
    LineRenderer _lineRenderer;
    Dictionary<float, Vector3> _points = new Dictionary<float, Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        
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
            float time = _points.Count > 0 ? _points.LastOrDefault().Key + Time.deltaTime : 0;
            position.z = 10;
            _points.Add(time, position);
            _lineRenderer.positionCount = _points.Count;
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, Camera.main.ScreenToWorldPoint(position));
        }
        if (Input.GetMouseButtonUp(0))
        {
        }
    }
}
