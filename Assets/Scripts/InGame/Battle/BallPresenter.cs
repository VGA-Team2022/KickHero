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
    [Tooltip("初期位置を定めるTransform")]
    [SerializeField] Transform _startTransform;
    [Header("デバッグ用項目")]
    [Tooltip("リセット")]
    [SerializeField] bool _reset = false;

    BallModel _ballModel;

    private BallModel BallModel
    {
        get
        {
            if (_ballModel == null)
            {
                //Debug.LogError("_ballModelが初期化されていません。 ");
                Vector3 position = _startTransform ? _startTransform.position : View.Position;
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
            if (!_view)
            {
                _view = FindObjectOfType<BallView>();
                if (!_view)
                {
                    Debug.LogError("BallViewが見つかりませんでした。");
                    return null;
                }
            }
            return _view;
        }
    }

    /// <summary>当たり判定を取るか否か</summary>
    public bool IsCollision { get => View.IsCollision; set { View.IsCollision = value; } }

    /// <summary>ボールの初期位置</summary>
    public Vector3 StartPosition
    {
        get
        {
            if (Application.isPlaying)
            {
                return BallModel.StartPosition;
            }
            else
            {
                return _startTransform.position;
            }
        }
        set
        {
            if (Application.isPlaying)
            {
                BallModel.StartPosition = value;
            }
            else
            {
                _startTransform.position =  value;
            }
        }
    }

    /// <summary>現在実行中の動作をキャンセルする</summary>
    public void Cancel()
    {
        BallModel.Cancel();
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
    public BallPresenter Shoot()
    {
        BallModel.Shoot();
        return this;
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

    public void Init(System.Action<InGameCycle.EventEnum> action)
    {
        if (View)
        {
            BallModel = new BallModel(value => View.Position = value, View.gameObject, View.transform.position, action); ;
        }
        ValueSet();
    }

    public void Init()
    {
        if (View)
        {
            BallModel = new BallModel(value => View.Position = value, View.gameObject, View.transform.position);
        }
        ValueSet();
    }

    void ValueSet()
    {
        if (BallModel == null) { return; }
        BallModel.Mode = _mode;
        BallModel.Acceleration = _acceleration;
        BallModel.Speed = _speed;
        BallModel.CalculationTime = _calculationTime;
    }

#if UNITY_EDITOR

    private void OnValidate()
    {
        ValueSet();
        if (_reset)
        {
            Reset();
        }
    }
    private void Reset()
    {
        //_view.
    }

#endif
}