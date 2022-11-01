using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;

public class BallModel
{


    ReactiveProperty<Vector3> _position = new ReactiveProperty<Vector3>();

    float _time = default;
    float _accele = default;
    bool _isCarry = default;
    float _speed = default;
    float _acceleration = default;
    CarryMode _mode = default;
    BallRoute _route = default;

    public CarryMode Mode { get => _mode; set => _mode = value; }
    public float Speed { get => _speed; set => _speed = value; }
    public float Acceleration { get => _acceleration; set => _acceleration = value; }
    public BallRoute Route { get => _route; }


    public BallModel(System.Action<Vector3> action, GameObject gameObject)
    {
        _position.Subscribe(action).AddTo(gameObject);
    }

    /// <summary>
    /// ÉãÅ[ÉgÇéwíËÇµÇ¬Ç¬î≠éÀÇ∑ÇÈ
    /// </summary>
    /// <param name="route"></param>
    /// <returns></returns>
    public bool Shoot(MonoBehaviour monoBehaviour)
    {
        monoBehaviour.StartCoroutine(Carry());
        return true;
    }

    public bool TryRouteSet(BallRoute route)
    {
        if (!_isCarry)
        {
            _route = route;
            return true;
        }
        return false;
    }

    async UniTask Carry2(CancellationToken cancellation_token)
    {
        await UniTask.Yield(PlayerLoopTiming.FixedUpdate, cancellation_token);

    }


    IEnumerator Carry()
    {
        _isCarry = true;
        if (_mode == CarryMode.Time)
        {
            _time = _route.MinTime;
            while (_time <= _route.MaxTime)
            {
                yield return new WaitForFixedUpdate();
                _time += Time.fixedDeltaTime * (_speed + _accele);
                _accele += _acceleration * Time.fixedDeltaTime;
                if (_route.TryGetPointInCaseTime(_time, out Vector3 point))
                {
                    //transform.position = point;
                    _position.Value = point;
                }
                else
                {
                    if (Mathf.Abs(_route.MinTime - _time) > Mathf.Abs(_route.MaxTime - _time))
                    {
                        //transform.position = _route.Positons.Last();
                        _position.Value = _route.Positons.Last();
                    }
                    else
                    {
                        //transform.position = _route.Positons.First();
                        _position.Value = _route.Positons.First();
                    }
                }
            }
        }
        else if (_mode == CarryMode.Distance)
        {
            _time = 0;
            while (_time <= _route.AllWay)
            {
                //Debug.Log($"{_time}, {_ballRoute.MaxTime}");
                yield return new WaitForFixedUpdate();
                _time += Time.fixedDeltaTime * (_speed + _accele);
                _accele += _acceleration * Time.fixedDeltaTime;
                if (_route.TryGetPointInCaseDistance(_time, out Vector3 point))
                {
                    //transform.position = point;
                    _position.Value = point;
                }
                else
                {
                    if (_time > _route.AllWay - _time)
                    {
                        //transform.position = _route.Positons.Last();
                        _position.Value = _route.Positons.Last();
                    }
                    else
                    {
                        //transform.position = _route.Positons.First();
                        _position.Value = _route.Positons.First();
                    }
                }
            }
        }
        _accele = 0;
        _isCarry = false;
    }


    public enum CarryMode
    {
        Time,
        Distance,
    }
}
