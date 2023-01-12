using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LineTracer : MonoBehaviour
{
    [SerializeField]
    GameObject _traceObject;
    [SerializeField]
    Transform[] _tracePaths;

    [SerializeField]
    float _traceTime = 2f;

    private void Start()
    {
        StartTrace();
    }
    public void StartTrace()
    {
        Vector3[] paths = new Vector3[_tracePaths.Length];
        for (int i = 0; i < _tracePaths.Length; i++)
        {
            paths[i] = _tracePaths[i].localPosition;
        }   
        _traceObject.transform.DOLocalPath(paths, _traceTime, PathType.Linear,PathMode.Sidescroller2D)
            .SetLoops(-1,LoopType.Restart);
    }

    public void StopTrace()
    {

    }
}
