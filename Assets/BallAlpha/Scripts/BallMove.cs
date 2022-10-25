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

    /// <summary>
    /// ƒ‹[ƒg‚ğw’è‚µ‚Â‚Â”­Ë‚·‚é
    /// </summary>
    /// <param name="route"></param>
    /// <returns></returns>
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
            while (_time <= _route.MaxTime)
            {
                yield return new WaitForFixedUpdate();
                _time += Time.fixedDeltaTime * (_speed + _accele);
                _accele += _acceleration * Time.fixedDeltaTime;
                if (_route.TryPointInCaseTime(_time, out Vector3 point))
                {
                    transform.position = point;
                }
            }
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
        }
        _accele = 0;
    }

    public enum CarryMode
    {
        Time,
        Distance,
    }
}
