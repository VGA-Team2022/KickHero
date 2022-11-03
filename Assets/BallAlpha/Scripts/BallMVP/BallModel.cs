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
    CancellationTokenSource _tokenSource = new CancellationTokenSource();

    float _time = default;
    float _accele = default;
    bool _isCarry = default;
    float _speed = default;
    float _acceleration = default;
    CarryMode _mode = default;
    BallRoute _route = default;
    Transform _startTransform = default;

    public CarryMode Mode { get => _mode; set => _mode = value; }
    public float Speed { get => _speed; set => _speed = value; }
    public float Acceleration { get => _acceleration; set => _acceleration = value; }
    public BallRoute Route { get => _route; }
    public Transform StartTransform { get => _startTransform;
        set
        {
            _startTransform = value;
            if (!_isCarry)
            {
                _position.Value = _startTransform.position;
            }
        }
    }

    public ReactiveProperty<Vector3> Position { get => _position;}

    public BallModel(System.Action<Vector3> action, GameObject gameObject)
    {
        _position.Subscribe(action).AddTo(gameObject);
    }

    public BallModel() { }

    ~BallModel()
    {
        _tokenSource.Cancel();
    }

    /// <summary>
    /// 発射する
    /// </summary>
    /// <param name="route"></param>
    /// <returns></returns>
    public bool Shoot()
    {
        //if(_source.Token.G)
        if(_route == null) { return false; }
        Carry().Forget();
        return true;
    }

    /// <summary>
    /// 現在の移動を終了する
    /// </summary>
    void Cancel()
    {
        _tokenSource?.Cancel();
        _isCarry = false;
    }

    /// <summary>
    /// Positionをサブスクライブする
    /// </summary>
    /// <param name="action"></param>
    /// <param name="gameObject"></param>
    public void Subscribe(System.Action<Vector3> action, GameObject gameObject)
    {
        _position.Subscribe(action).AddTo(gameObject);
    }
    /// <summary>
    /// Positionをサブスクライブする
    /// </summary>
    /// <param name="action"></param>
    /// <param name="component"></param>
    public void Subscribe(System.Action<Vector3> action, Component component)
    {
        _position.Subscribe(action).AddTo(component);
    }

    /// <summary>
    /// 初期位置に戻る
    /// </summary>
    public void Collection()
    {
        if (_startTransform)
        {
            _position.Value = _startTransform.position;
        }
    }
    /// <summary>
    /// ルートの設定を試みる
    /// 飛行中は再設定できない
    /// </summary>
    /// <param name="route"></param>
    /// <returns></returns>
    public bool TryRouteSet(BallRoute route)
    {
        if (!_isCarry)
        {
            _route = route;
            return true;
        }
        return false;
    }



    
    async UniTask Carry()
    {
        _isCarry = true;
        if (_mode == CarryMode.Time)
        {
            _time = _route.MinTime;
            while (_time <= _route.MaxTime)
            {
                //yield return new WaitForFixedUpdate();
                
                 await UniTask.Yield(PlayerLoopTiming.FixedUpdate, _tokenSource.Token);
                if(this == null)
                {
                    Debug.Log(5);
                }
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

                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, _tokenSource.Token);
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
