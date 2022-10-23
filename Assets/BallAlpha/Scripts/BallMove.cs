using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    [SerializeField] float _speed = 10;

    float _time = 0;
    Dictionary<float, Vector3> _route;

    BallRoute _ballRoute;

    public bool Shoot(BallRoute route)
    {
        _ballRoute = route;
        StartCoroutine(Carry());
        return true;
    }
    private void Start()
    {
    }

    private void Update()
    {
    }

    IEnumerator Carry()
    {
        while (_time >= _ballRoute.AllTime)
        {
            yield return null;
            if (_ballRoute.TryPointInCaseTime(_time, out Vector3 point))
            {
                transform.position = point;
            }
        }
    }
}
