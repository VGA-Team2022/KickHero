using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;

public class BallModel
{

    /// <summary>�{�[����Position</summary>
    ReactiveProperty<Vector3> _position = new ReactiveProperty<Vector3>();
    /// <summary>�����̃g�[�N��</summary>
    CancellationTokenSource _tokenSource = new CancellationTokenSource();
    /// <summary>�i�s��</summary>
    float _progressStatus = default;
    /// <summary>������</summary>
    float _accele = default;
    /// <summary>���s�����ۂ�</summary>
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

    //public ReactiveProperty<Vector3> Position { get => _position;}

    public BallModel(System.Action<Vector3> action, GameObject gameObject)
    {
        _position.Subscribe(action).AddTo(gameObject);
    }

    public BallModel() { }

    ~BallModel()
    {
        _tokenSource?.Cancel();
    }

    /// <summary>
    /// ���˂���
    /// </summary>
    /// <param name="route"></param>
    /// <returns></returns>
    public bool Shoot()
    {
        _tokenSource = new CancellationTokenSource();
        if(_route == null) { return false; }
        Carry().Forget();
        return true;
    }

    /// <summary>
    /// ���݂̈ړ����I������
    /// </summary>
    void Cancel()
    {
        _tokenSource?.Cancel();
        _isCarry = false;
    }

    /// <summary>
    /// Position���T�u�X�N���C�u����
    /// </summary>
    /// <param name="action"></param>
    /// <param name="gameObject"></param>
    public void Subscribe(System.Action<Vector3> action, GameObject gameObject)
    {
        _position.Subscribe(action).AddTo(gameObject);
    }
    /// <summary>
    /// Position���T�u�X�N���C�u����
    /// </summary>
    /// <param name="action"></param>
    /// <param name="component"></param>
    public void Subscribe(System.Action<Vector3> action, Component component)
    {
        _position.Subscribe(action).AddTo(component);
    }

    /// <summary>
    /// �����ʒu�ɖ߂�
    /// </summary>
    public void Collection()
    {
        if (_startTransform)
        {
            _position.Value = _startTransform.position;
        }
    }
    /// <summary>
    /// ���[�g�̐ݒ�����݂�
    /// ��s���͍Đݒ�ł��Ȃ�
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
            _progressStatus = _route.MinTime;
            while (_progressStatus <= _route.MaxTime)
            {
                //yield return new WaitForFixedUpdate();
                
                 await UniTask.Yield(PlayerLoopTiming.FixedUpdate, _tokenSource.Token);
                if(this == null)
                {
                    Debug.Log(5);
                }
                _progressStatus += Time.fixedDeltaTime * (_speed + _accele);
                _accele += _acceleration * Time.fixedDeltaTime;
                if (_route.TryGetPointInCaseTime(_progressStatus, out Vector3 point))
                {
                    //transform.position = point;
                    _position.Value = point;
                }
                else
                {
                    if (Mathf.Abs(_route.MinTime - _progressStatus) > Mathf.Abs(_route.MaxTime - _progressStatus))
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
            _progressStatus = 0;
            while (_progressStatus <= _route.AllWay)
            {
                //Debug.Log($"{_time}, {_ballRoute.MaxTime}");

                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, _tokenSource.Token);
                _progressStatus += Time.fixedDeltaTime * (_speed + _accele);
                _accele += _acceleration * Time.fixedDeltaTime;
                if (_route.TryGetPointInCaseDistance(_progressStatus, out Vector3 point))
                {
                    //transform.position = point;
                    _position.Value = point;
                }
                else
                {
                    if (_progressStatus > _route.AllWay - _progressStatus)
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
