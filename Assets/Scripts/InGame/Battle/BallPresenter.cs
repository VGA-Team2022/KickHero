using ModestTree.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject.SpaceFighter;

public class BallPresenter : MonoBehaviour
{
    [Tooltip("ボールのView")]
    [SerializeField] BallView _view;
    [Tooltip("ボールの速さ")]
    [SerializeField] float _speed = 1;
    [Tooltip("ボールの加速度")]
    [SerializeField] float _acceleration = 0;
    [Tooltip("最後にボールを飛ばす速度の算出に使う長さ")]
    [SerializeField] float _calculationTime = 0;
    [Tooltip("ボールの速度モード")]
    [SerializeField] BallModel.CarryMode _mode = BallModel.CarryMode.Time;

    BallModel _ballModel;

    private BallModel BallModel
    {
        get
        {
            if (_ballModel == null)
            {
                //Debug.LogError("_ballModelが初期化されていません。");
                _ballModel = new BallModel(_view.transform.position);
            }
            return _ballModel;
        }
        set => _ballModel = value;
    }

    private BallView View
    {
        get
        {
            _view = FindObjectOfType<BallView>();
            if (!_view)
            {
                Debug.LogError("BallViewが見つかりませんでした。");
            }
            return _view;
        }
    }

    /// <summary>当たり判定を取るか否か</summary>
    public bool IsCollision { get => View.IsCollision; set { View.IsCollision = value; Debug.Log(value); } }

    /// <summary>現在実行中の動作をキャンセルする</summary>
    public void Cancel()
    {
        BallModel.Cancel();
        if (View)
        {
            View.Rigidbody.useGravity = false;
            View.Rigidbody.velocity= Vector3.zero;
        }
    }

    /// <summary>初期位置に戻る</summary>
    public void Collection()
    {
        BallModel.Collection();
    }

    /// <summary>ルートの設定を試みる
    /// ボールの状況によっては設定できない</summary>
    public bool TryRouteSet(BallRoute route)
    {
        return BallModel.TryRouteSet(route);
    }

    /// <summary>ボールを発射する</summary>
    public void Shoot()
    {
        BallModel.Shoot();
    }

    /// <summary>ルートを辿り終えた時に呼ぶアクションを設定する</summary>
    public BallPresenter OnCarryEnd(Action action)
    {
        return OnCarryEnd(action, true);
    }
    /// <summary>ルートを辿り終えた時に呼ぶアクションを設定する</summary>
    public BallPresenter OnCarryEnd(Action action, bool reusable)
    {
        BallModel.OnCarryEnd(action, reusable);
        return this;
    }

    /// <summary>ボールがヒットした時に呼ばれるアクションを設定する</summary>
    public BallPresenter OnHit(Action<Collider> action)
    {
        View?.OnHit(action);
        return this;
    }

    /// <summary>ボールを隠す</summary>
    public void Hide()
    {
        View?.Hide();
    }

    /// <summary>ボールを出す</summary>
    public void Display()
    {
        View?.Display();
    }

    private void Start()
    {
        Init();
    }

    private void OnValidate()
    {
        ValueSet();
    }

    public void Init(System.Action<InGameCycle.EventEnum> action)
    {
        if (View)
        {
            _ballModel = new BallModel(
            value =>
            {
                _view.Position = value;
            }, value =>
            {
                _view.Rigidbody.velocity = value;
            },
            _view.gameObject, _view.transform.position
            , action); ;
        }
        ValueSet();
    }

    public void Init()
    {
        if (View)
        {
            _ballModel = new BallModel(
            value =>
            {
                _view.Position = value;
            }, value =>
            {
                _view.Rigidbody.velocity = value;
            },
            _view.gameObject, _view.transform.position
            ).OnCarryEnd(() => { _view.Rigidbody.useGravity = true; }) ;
        }
        ValueSet();
    }

    void ValueSet()
    {
        if (_ballModel == null) { return; }
        _ballModel.Mode = _mode;
        _ballModel.Acceleration = _acceleration;
        _ballModel.Speed = _speed;
        _ballModel.CalculationTime = _calculationTime;
    }
}