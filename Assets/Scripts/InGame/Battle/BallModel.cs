using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;
using Unity.VisualScripting;
using System;


/// <summary>
/// ï¿½{ï¿½[ï¿½ï¿½ï¿½Ìƒfï¿½[ï¿½^ï¿½Iï¿½Èï¿½ï¿½ï¿½ï¿½ÆAï¿½fï¿½[ï¿½^ï¿½ï¿½Ûï¿½ï¿½ï¿½ï¿½ï¿½Nï¿½ï¿½ï¿½X
/// </summary>
public class BallModel
{

    /// <summary>ƒ{[ƒ‹‚ÌPosition</summary>
    ReactiveProperty<Vector3> _position;
    ReactiveProperty<InGameCycle.EventEnum> _eventProperty;
    /// <summary>ˆ—‚Ìƒg[ƒNƒ“</summary>
    CancellationTokenSource _tokenSource = new CancellationTokenSource();
    /// <summary>isó‹µ</summary>
    float _progressStatus = default;
    /// <summary>‰Á‘¬•ª</summary>
    float _accele = default;
    /// <summary>Às’†‚©”Û‚©</summary>
    bool _isCarry = default;
    /// <summary>ƒ‹[ƒg‚ğ’H‚èI‚¦‚½‚Æ‚«‚ÉŒÄ‚ÔƒAƒNƒVƒ‡ƒ“</summary>
    System.Action _onCarryEndAction;
    /// <summary>ƒ‹[ƒg‚ğ’H‚èI‚¦‚½‚Æ‚«‚ÉŒÄ‚ÔƒAƒNƒVƒ‡ƒ“i‚Â‚©‚¢‚·‚Äj</summary>
    System.Action _singleUseOnCarryEndAction;
    /// <summary>ƒ{[ƒ‹‚Ì‘¬‚³</summary>
    float _speed = default;
    /// <summary>ƒ{[ƒ‹‚Ì‰Á‘¬“x(ƒXƒJƒ‰[)</summary>
    float _acceleration = default;
    /// <summary>ƒ{[ƒ‹‚Ì‘¬“x‚Ìƒ‚[ƒh</summary>
    CarryMode _mode = default;
    /// <summary>ƒ{[ƒ‹‚ª’H‚éƒ‹[ƒg</summary>
    BallRoute _route = default;
    /// <summary>‰ŠúˆÊ’u</summary>
    Vector3 _startPosition = default;
    /// <summary>ÅŒã‚Éƒ{[ƒ‹‚ğ”ò‚Î‚·‘¬“x‚ÌŒvZ‚ÉŠÜ‚ß‚éŠÔ</summary>
    float _calculationTime = default;
    /// <summary>ƒfƒoƒbƒOƒ‚[ƒh‚©‚Ç‚¤‚©</summary>
    bool _isDebug = false;
    /// <summary>’n–Ê‚Ìƒ^ƒO‚Ì–¼‘O</summary>
    string _groundTag;
    Vector3 _velocity = default;
    bool _isCarryEnd = false;
    float _radius;
    PhysicMaterial _physicMaterial;

    public CarryMode Mode { get => _mode; set => _mode = value; }
    public float Speed { get => _speed; set => _speed = value; }
    public float Acceleration { get => _acceleration; set => _acceleration = value; }
    public BallRoute Route { get => _route; }
    public float CalculationTime { get => _calculationTime; set => _calculationTime = value; }
    public Vector3 StartPosition { get => _startPosition; set => _startPosition = value; }
    public Vector3 Position { get => _position.Value; set => _position.Value = value; }
    public string GroundTag { get => _groundTag; set => _groundTag = value; }
    public float Radius { get => _radius; set => _radius = value; }
    public Vector3 Velocity { get => _velocity; set { _velocity = value; } }

    public PhysicMaterial PhysicMaterial { get => _physicMaterial; set => _physicMaterial = value; }


    //public ReactiveProperty<Vector3> Position { get => _position;}

    public BallModel(Action<Vector3> position, GameObject gameObject, Vector3 startPosition, System.Action<InGameCycle.EventEnum> eventAction)
    {
        _startPosition = startPosition;
        _position = new ReactiveProperty<Vector3>(_startPosition);
        _position.Subscribe(position).AddTo(gameObject);

        //ƒV[ƒPƒ“ƒX‚Ì‘JˆÚ‚ğw’èB
        _eventProperty = new ReactiveProperty<InGameCycle.EventEnum>(InGameCycle.EventEnum.None);
        _eventProperty.Subscribe(eventAction).AddTo(gameObject);
    }

    public BallModel(Action<Vector3> position, GameObject gameObject, Vector3 startPosition)
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
        Cancel();
    }

    /// <summary>
    /// ”­Ë‚·‚é
    /// </summary>
    /// <param name="route"></param>
    /// <returns></returns>
    public bool Shoot()
    {
        if (!_isDebug)
        {
            _eventProperty.Value = InGameCycle.EventEnum.Throw;
        }
        Cancel();
        _tokenSource = new CancellationTokenSource();
        if (_route == null) { return false; }
        Carry().Forget();
        return true;
    }

    /// <summary>
    /// Œ»İ‚ÌˆÚ“®‚ğI—¹‚·‚é
    /// </summary>
    public void Cancel()
    {
        _tokenSource?.Cancel();
        _isCarry = false;
        _accele = 0;
        Velocity = Vector3.zero;
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
    /// Position‚ğƒTƒuƒXƒNƒ‰ƒCƒu‚·‚é
    /// </summary>
    /// <param name="action"></param>
    /// <param name="gameObject"></param>
    public void PositionSubscribe(Action<Vector3> action, GameObject gameObject)
    {
        _position.Subscribe(action).AddTo(gameObject);
    }
    /// <summary>
    /// Position‚ğƒTƒuƒXƒNƒ‰ƒCƒu‚·‚é
    /// </summary>
    /// <param name="action"></param>
    /// <param name="component"></param>
    public void PositionSubscribe(Action<Vector3> action, Component component)
    {
        _position.Subscribe(action).AddTo(component);
    }

    /// <summary>
    /// ‰ŠúˆÊ’u‚É–ß‚é
    /// </summary>
    public void Collection()
    {
        _position.Value = _startPosition;
        Cancel();
    }
    /// <summary>
    /// ƒ‹[ƒg‚Ìİ’è‚ğ‚İ‚é
    /// ”òs’†‚ÍÄİ’è‚Å‚«‚È‚¢
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

    public void OnRaycastHit(RaycastHit hit)
    {
        if (hit.collider.tag == _groundTag)
        {
            _isCarryEnd = true;
            Debug.DrawRay(Position, Velocity, Color.red);
            Debug.Log(Velocity);
            Velocity = Velocity + 2 * -Vector3.Dot(hit.normal, Velocity) * hit.normal;
            Debug.Log(Velocity);
            Debug.DrawRay(hit.point, Velocity, Color.blue);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
            CallOnCarryEnd();
            _position.Value = hit.point + hit.normal * _radius;
            //UnityEditor.EditorApplication.isPaused = true;
        }
    }


    async UniTask Carry()
    {
        _isCarry = true;
        _isCarryEnd = false;
        if (_mode == CarryMode.Time)
        {
            _progressStatus = _route.MinTime;
            while (_progressStatus <= _route.MaxTime)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, _tokenSource.Token);
                if (_isCarryEnd) { break; }
                float buf = _progressStatus;
                _progressStatus += Time.deltaTime * (_speed + _accele);
                _accele += _acceleration * Time.deltaTime;
                if (_route.TryGetPointInCaseTime(_progressStatus, out Vector3 point))
                {
                    Velocity = _route.GetVelocityInCaseTime(buf, _progressStatus).Value;
                    _position.Value = point;
                }
                else
                {
                    if (Mathf.Abs(_route.MinTime - _progressStatus) > Mathf.Abs(_route.MaxTime - _progressStatus))
                    {
                        _position.Value = _route.Positons.Last();
                        float time = _calculationTime < _route.AllTime ? _route.MaxTime - _calculationTime : _route[0].Time;
                        Velocity = _route.GetVelocityInCaseTime(time, _route.MaxTime).Value;
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
                if (_isCarryEnd) { break; }
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
                        Velocity = pos - _position.Value;
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
        while (Velocity.sqrMagnitude != 0)
        {
            Velocity += Physics.gravity * Time.fixedDeltaTime;
            _position.Value += Velocity * Time.fixedDeltaTime;
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate, _tokenSource.Token);
        }
    }

    float GetBounciness(PhysicMaterial a, PhysicMaterial b)
    {
        float bounciness = 1;
        if (a)
        {
            bounciness = a.bounciness;
        }

        return 0;
    }


    public enum CarryMode
    {
        Time,
        Distance,
    }
}
