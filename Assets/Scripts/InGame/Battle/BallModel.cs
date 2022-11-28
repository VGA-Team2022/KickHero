using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;
using Unity.VisualScripting;
using System;

public class BallModel
{

    /// <summary>ボールのPosition</summary>
    ReactiveProperty<Vector3> _position;
    ReactiveProperty<InGameCycle.EventEnum> _eventProperty;
    /// <summary>処理のトークン</summary>
    CancellationTokenSource _tokenSource = new CancellationTokenSource();
    /// <summary>進行状況</summary>
    float _progressStatus = default;
    /// <summary>加速分</summary>
    float _accele = default;
    /// <summary>実行中か否か</summary>
    bool _isCarry = default;
    /// <summary>ルートを辿り終えたときに呼ぶアクション</summary>
    System.Action _onCarryEndAction;
    /// <summary>ルートを辿り終えたときに呼ぶアクション（つかいすて）</summary>
    System.Action _singleUseOnCarryEndAction;
    float _speed = default;
    float _acceleration = default;
    CarryMode _mode = default;
    BallRoute _route = default;
    //Transform _startTransform = default;
    /// <summary>初期位置</summary>
    Vector3 _startPosition = default;
    float _calculationTime = default;

    bool _isDebug = false;

    public CarryMode Mode { get => _mode; set => _mode = value; }
    public float Speed { get => _speed; set => _speed = value; }
    public float Acceleration { get => _acceleration; set => _acceleration = value; }
    public BallRoute Route { get => _route; }
    public float CalculationTime { get => _calculationTime; set => _calculationTime = value; }
    public Vector3 StartPosition { get => _startPosition; set => _startPosition = value; }


    //public ReactiveProperty<Vector3> Position { get => _position;}

    public BallModel(System.Action<Vector3> position, GameObject gameObject, Vector3 startPosition, System.Action<InGameCycle.EventEnum> eventAction)
    {
        _startPosition = startPosition;
        _position = new ReactiveProperty<Vector3>(_startPosition);
        _position.Subscribe(position).AddTo(gameObject);

        //シーケンスの遷移を指定。
        _eventProperty = new ReactiveProperty<InGameCycle.EventEnum>(InGameCycle.EventEnum.None);
        _eventProperty.Subscribe(eventAction).AddTo(gameObject);
    }

    public BallModel(System.Action<Vector3> position, GameObject gameObject, Vector3 startPosition)
    {
        _startPosition = startPosition;
        _position = new ReactiveProperty<Vector3>(_startPosition);
        _position.Subscribe(position).AddTo(gameObject);
        _isDebug = true;
    }

    public BallModel(Vector3 startPosition)
    {
        _startPosition = startPosition;
        _position = new ReactiveProperty<Vector3>(_startPosition);
        _isDebug = true;
    }

    ~BallModel()
    {
        _tokenSource?.Cancel();
    }

    /// <summary>
    /// 発射する
    /// </summary>
    /// <param name="route"></param>
    /// <returns></returns>
    public bool Shoot()
    {
        if (!_isDebug)
        {
            _eventProperty.Value = InGameCycle.EventEnum.Throw;
        }
        _tokenSource?.Cancel();
        _tokenSource = new CancellationTokenSource();
        if (_route == null) { return false; }
        Carry().Forget();
        return true;
    }

    /// <summary>
    /// 現在の移動を終了する
    /// </summary>
    public void Cancel()
    {
        _tokenSource?.Cancel();
        _isCarry = false;
        _accele = 0;
    }

    public BallModel OnCarryEnd(System.Action action)
    {
        return OnCarryEnd(action, true);
    }

    public BallModel OnCarryEnd(System.Action action, bool reusable)
    {
        if (reusable)
        {
            _onCarryEndAction += action;
        }
        else
        {
            _singleUseOnCarryEndAction += action;
        }
        return this;
    }

    void CallOnCarryEnd()
    {
        _onCarryEndAction?.Invoke();
        _singleUseOnCarryEndAction?.Invoke();
        _singleUseOnCarryEndAction = null;
    }

    /// <summary>
    /// Positionをサブスクライブする
    /// </summary>
    /// <param name="action"></param>
    /// <param name="gameObject"></param>
    public void PositionSubscribe(System.Action<Vector3> action, GameObject gameObject)
    {
        _position.Subscribe(action).AddTo(gameObject);
    }
    /// <summary>
    /// Positionをサブスクライブする
    /// </summary>
    /// <param name="action"></param>
    /// <param name="component"></param>
    public void PositionSubscribe(System.Action<Vector3> action, Component component)
    {
        _position.Subscribe(action).AddTo(component);
    }

    /// <summary>
    /// 初期位置に戻る
    /// </summary>
    public void Collection()
    {
        _position.Value = _startPosition;
        Cancel();
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
        Vector3 velo = Vector3.zero;
        if (_mode == CarryMode.Time)
        {
            _progressStatus = _route.MinTime;
            while (_progressStatus <= _route.MaxTime)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, _tokenSource.Token);

                float buf = _progressStatus;
                _progressStatus += Time.deltaTime * (_speed + _accele);
                _accele += _acceleration * Time.deltaTime;
                if (_route.TryGetPointInCaseTime(_progressStatus, out Vector3 point))
                {
                    _position.Value = point;
                }
                else
                {
                    if (Mathf.Abs(_route.MinTime - _progressStatus) > Mathf.Abs(_route.MaxTime - _progressStatus))
                    {
                        _position.Value = _route.Positons.Last();
                        float time = _calculationTime < _route.AllTime ? _route.MaxTime - _calculationTime : _route[0].Time;
                        velo = _route.GetVelocityInCaseTime(time, _route.MaxTime).Value;
                    }
                    else
                    {
                        _position.Value = _route.Positons.First();
                    }
                }
            }
        }
        else if (_mode == CarryMode.Distance)
        {
            _progressStatus = 0;
            while (_progressStatus <= _route.AllWay)
            {

                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, _tokenSource.Token);
                _progressStatus += Time.fixedDeltaTime * (_speed + _accele);
                _accele += _acceleration * Time.fixedDeltaTime;
                if (_route.TryGetPointInCaseDistance(_progressStatus, out Vector3 point))
                {
                    _position.Value = point;
                }
                else
                {
                    if (_progressStatus > _route.AllWay - _progressStatus)
                    {
                        Vector3 pos = _route.Positons.Last();
                        velo = pos - _position.Value;
                        _position.Value = pos;
                    }
                    else
                    {
                        _position.Value = _route.Positons.First();
                    }
                }
            }
        }
        CallOnCarryEnd();
        if (!_isDebug)
        {
            _eventProperty.Value = InGameCycle.EventEnum.BallRespawn;
        }
        while (velo.sqrMagnitude != 0)
        {
            velo += Physics.gravity * Time.deltaTime;
            _position.Value += velo * Time.deltaTime;
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate, _tokenSource.Token);
        }
    }


    public enum CarryMode
    {
        Time,
        Distance,
    }
}
