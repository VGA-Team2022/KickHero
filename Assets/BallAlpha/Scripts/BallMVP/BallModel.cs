using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;
using Unity.VisualScripting;

public class BallModel
{

    /// <summary>�{�[����Position</summary>
    ReactiveProperty<Vector3> _position;
    ReactiveProperty<Vector3> _velocity = new ReactiveProperty<Vector3>();
    /// <summary>�����̃g�[�N��</summary>
    CancellationTokenSource _tokenSource = new CancellationTokenSource();
    /// <summary>�i�s��</summary>
    float _progressStatus = default;
    /// <summary>������</summary>
    float _accele = default;
    /// <summary>���s�����ۂ�</summary>
    bool _isCarry = default;
    System.Action _onCarryEndAction;
    float _speed = default;
    float _acceleration = default;
    CarryMode _mode = default;
    BallRoute _route = default;
    Transform _startTransform = default;
    float _calculationTime = default;

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
    public float CalculationTime { get => _calculationTime; set => _calculationTime = value; }


    //public ReactiveProperty<Vector3> Position { get => _position;}

    public BallModel(System.Action<Vector3> action, GameObject gameObject, Transform startPosition)
    {
        _startTransform = startPosition;
        _position = new ReactiveProperty<Vector3>(_startTransform.position);
        _position.Subscribe(action).AddTo(gameObject);
    }

    public BallModel(Transform startPosition)
    {
        _startTransform = startPosition;
        _position = new ReactiveProperty<Vector3>(_startTransform.position);
    }

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
        _tokenSource?.Cancel();
        _tokenSource = new CancellationTokenSource();
        if(_route == null) { return false; }
        Carry().Forget();
        return true;
    }

    /// <summary>
    /// ���݂̈ړ����I������
    /// </summary>
    public void Cancel()
    {
        _tokenSource?.Cancel();
        _isCarry = false;
        _accele = 0;
    }

    public BallModel OnCarryEnd(System.Action action)
    {
        _onCarryEndAction += action;
        return this;
    }

    void CallOnCarryEnd()
    {
        _onCarryEndAction?.Invoke();
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
        Cancel();
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
        Vector3 velo = Vector3.zero;
        if (_mode == CarryMode.Time)
        {
            _progressStatus = _route.MinTime;
            while (_progressStatus <= _route.MaxTime)
            {
                //yield return new WaitForFixedUpdate();
                
                 await UniTask.Yield(PlayerLoopTiming.Update, _tokenSource.Token);
                if(this == null)
                {
                    Debug.Log(5);
                }
                _progressStatus += Time.deltaTime * (_speed + _accele);
                _accele += _acceleration * Time.deltaTime;
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
                        Vector3 pos = _route.Positons.Last();
                        float time = _calculationTime < _route.AllTime ? _route.MaxTime - _calculationTime : _route[0].Time;
                        velo = (pos - _route.GetPointInCaseTime(time).Value) / (_route[_route.Count - 1].Time - time);
                        Debug.DrawRay(_position.Value, velo, Color.green);
                        //Debug.DrawLine(pos, _route[_route.Count - 2].Point, Color.blue);
                        //Debug.DrawLine(_position.Value, pos, Color.red);
                        //UnityEditor.EditorApplication.isPaused = true;
                        _position.Value = pos;
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

                await UniTask.Yield(PlayerLoopTiming.Update, _tokenSource.Token);
                _progressStatus += Time.deltaTime * (_speed + _accele);
                _accele += _acceleration * Time.deltaTime;
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
                        Vector3 pos = _route.Positons.Last();
                        velo = pos - _position.Value;
                        _position.Value = pos;
                    }
                    else
                    {
                        //transform.position = _route.Positons.First();
                        _position.Value = _route.Positons.First();
                    }
                }
            }
        }
        CallOnCarryEnd();
        Debug.Log(2);
        if (velo.sqrMagnitude != 0)
        {
            _velocity.Value = velo;
        }
        //while (velo.sqrMagnitude != 0)
        //{
        //    velo += Physics.gravity * Time.deltaTime;
        //    _position.Value += velo * Time.deltaTime;
        //    await UniTask.Yield(PlayerLoopTiming.Update, _tokenSource.Token);
        //}
    }


    public enum CarryMode
    {
        Time,
        Distance,
    }
}
