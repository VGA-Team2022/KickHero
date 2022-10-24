using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    [SerializeField] float _speed = 1;
    [SerializeField] float _acceleration = 0;
    [SerializeField] CarryMode _mode;

    float _time = 0;
    float _accele = 0;

    BallRoute _route;

    public CarryMode Mode { get => _mode; set => _mode = value; }
    public float Speed { get => _speed; set => _speed = value; }
    public float Acceleration { get => _acceleration; set => _acceleration = value; }

    public bool Shoot(BallRoute route)
    {
        _route = route;
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
        if (_mode == CarryMode.Time)
        {
            _time = _route.MinTime;
            Debug.Log(_route.AllTime);
            while (_time <= _route.MaxTime)
            {
                //Debug.Log($"{_time}, {_ballRoute.MaxTime}");
                yield return new WaitForFixedUpdate();
                _time += Time.fixedDeltaTime * (_speed + _accele);
                _accele += _acceleration * Time.fixedDeltaTime;
                Debug.Log(_accele);
                if (_route.TryPointInCaseTime(_time, out Vector3 point))
                {
                    transform.position = point;
                }
            }
            Debug.Log(_time - _route.MinTime);
        }
        else if(_mode == CarryMode.Distance)
        {
            _time = 0;
            while (_time <= _route.AllWay)
            {
                //Debug.Log($"{_time}, {_ballRoute.MaxTime}");
                yield return new WaitForFixedUpdate();
                _time += Time.fixedDeltaTime * (_speed + _accele);
                _accele += _acceleration * Time.fixedDeltaTime;
                if (_route.TryPointInCaseDistance(_time, out Vector3 point))
                {
                    transform.position = point;
                }
            }
            //Debug.Log(1);
            //if (_route.TryPointInCaseDistance(_route.AllWay - 1, out Vector3 point)){
            //    transform.position = point;
            //}
        }
        _accele = 0;
    }

    public enum CarryMode
    {
        Time,
        Distance,
    }
}
