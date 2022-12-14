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
/// ボールのデータ的な処理と、データを保持するクラス
/// </summary>
public class BallModel
{

    /// <summary>ボールのPosition</summary>
    ReactiveProperty<Vector3> _position;
    ReactiveProperty<bool> _isKinematic;
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
    /// <summary>ボールの速さ</summary>
    float _speed = default;
    /// <summary>ボールの加速度(スカラー)</summary>
    float _acceleration = default;
    /// <summary>ボールの速度のモード</summary>
    CarryMode _mode = default;
    /// <summary>ボールが辿るルート</summary>
    BallRoute _route = default;
    /// <summary>初期位置</summary>
    Vector3 _startPosition = default;
    /// <summary>最後にボールを飛ばす速度の計算に含める時間</summary>
    float _calculationTime = default;
    /// <summary>デバッグモードかどうか</summary>
    bool _isDebug = false;
    /// <summary>地面のタグの名前</summary>
    string _groundTag;
    Vector3 _velocity = default;
    bool _isCarryEnd = false;
    float _radius;
    Collider _collider;
    Rigidbody _rb;
    float _missBoundSpeed = 0f;

    public CarryMode Mode { get => _mode; set => _mode = value; }
    public float Speed { get => _speed; set => _speed = value; }
    public float Acceleration { get => _acceleration; set => _acceleration = value; }
    public BallRoute Route { get => _route; }
    public float CalculationTime { get => _calculationTime; set => _calculationTime = value; }
    public Vector3 StartPosition { get => _startPosition; set => _startPosition = value; }
    public Vector3 Position
    {
        get => _position.Value;
        set => _position.Value = value;
    }
    public string GroundTag { get => _groundTag; set => _groundTag = value; }
    public float Radius { get => _radius; set => _radius = value; }
    public Vector3 Velocity
    {
        get
        {
            if (_rb && !_rb.isKinematic)
            {
                return _rb.velocity;
            }
            else
            {
                return _velocity;
            }
        }
        set
        {
            _velocity = value;
            if (_rb && !_rb.isKinematic)
            {
                _rb.velocity = value;
            }
        }
    }

    public Rigidbody Rigidbody
    {
        get => _rb; set
        {
            _rb = value;
            _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            Collection();
        }
    }
    public Collider Collider { get => _collider; set => _collider = value; }
    public float MissBoundSpeed { get => _missBoundSpeed; set => _missBoundSpeed = value; }


    //public ReactiveProperty<Vector3> Position { get => _position; }

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
    /// 発射する
    /// </summary>
    /// <param name="route"></param>
    /// <returns></returns>
    public bool Shoot()
    {
        if (_rb)
        {
            _rb.velocity = Vector3.zero;
            //_rb.isKinematic = true;
        }
        Cancel();
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
        _isCarryEnd = true;
        CallOnCarryEnd();
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
    public void PositionSubscribe(Action<Vector3> action, GameObject gameObject)
    {
        _position.Subscribe(action).AddTo(gameObject);
    }
    /// <summary>
    /// Positionをサブスクライブする
    /// </summary>
    /// <param name="action"></param>
    /// <param name="component"></param>
    public void PositionSubscribe(Action<Vector3> action, Component component)
    {
        _position.Subscribe(action).AddTo(component);
    }

    /// <summary>
    /// 初期位置に戻る
    /// </summary>
    public void Collection()
    {
        _position.Value = _startPosition;
        if (_rb)
        {
            //_rb.isKinematic = true;
            //_rb.velocity = Vector3.zero;
            _rb.useGravity = false;
            _rb.Sleep();
        }
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

    public void OnRaycastHit(RaycastHit hit)
    {
        if (hit.collider.tag == _groundTag)
        {
            MissBound(hit.point, hit.normal);
        }
    }
    public void OnCollision(Collision collision)
    {
        if (collision.collider.tag == _groundTag)
        {
            MissBound(collision.contacts[0].point, collision.contacts[0].normal);
        }
    }

    void MissBound(Vector3 point, Vector3 normal)
    {
        _isCarryEnd = true;
        //_rb.isKinematic = false;
        float x = UnityEngine.Random.value;
        float y = UnityEngine.Random.Range(-1f, 1);
        float z = UnityEngine.Random.Range(-1f, 1);
        Vector3 up = normal * y;
        Vector3 f = Vector3.Cross(normal, Vector3.right).normalized;
        Vector3 right = f * x;
        Vector3 forward = Vector3.Cross(normal, f).normalized * z;
        Velocity = (right + up + forward).normalized * _missBoundSpeed;
        _position.Value = point + normal * _radius;
        Cancel();
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
                if (_isCarryEnd) { break; }
                float buf = _progressStatus;
                _progressStatus += Time.fixedDeltaTime * (_speed + _accele);
                _accele += _acceleration * Time.fixedDeltaTime;
                if (_route.TryGetVelocityInCaseTime(buf, _progressStatus, out Vector3 velosity))
                {
                    Velocity = velosity;
                    _position.Value = _route.GetPointInCaseTime(buf).Value;
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
                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, _tokenSource.Token);
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
        if (_rb)
        {
            //_rb.isKinematic = false;
        }
        CallOnCarryEnd();
    }

    float GetBounciness(PhysicMaterial a, PhysicMaterial b)
    {
        if (!a) { a = new PhysicMaterial(); }
        if (!b) { b = new PhysicMaterial(); }
        float bounciness = 1;
        int combineIndx = Mathf.Max((int)a.bounceCombine, (int)b.bounceCombine);

        switch ((PhysicMaterialCombine)combineIndx)
        {
            case PhysicMaterialCombine.Average:
                bounciness = (a.bounciness + b.bounciness) / 2;
                break;
            case PhysicMaterialCombine.Minimum:
                bounciness = Mathf.Min(a.bounciness, b.bounciness);
                break;
            case PhysicMaterialCombine.Maximum:
                bounciness = Mathf.Max(a.bounciness, b.bounciness);
                break;
            case PhysicMaterialCombine.Multiply:
                bounciness = a.bounciness * b.bounciness;
                break;
        }

        return bounciness;
    }


    public enum CarryMode
    {
        Time,
        Distance,
    }
}
