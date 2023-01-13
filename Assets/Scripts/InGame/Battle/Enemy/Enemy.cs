using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Enemy
{
    EnemyHPPresenter _hpPresenter;
    EnemyBehaviorPresenter _behaviorPresenter;

    IDamage _playerDamage;
    public bool IsDead => _hpPresenter.IsDead;
    public Enemy(IDamage playerDamage)
    {
        Initialize();
        _playerDamage = playerDamage;
    }
    public void Initialize()
    {
        if (!_hpPresenter)
        {
            _hpPresenter = GetMonoBehaviorInstansInScene<EnemyHPPresenter>();
        }
        if (!_behaviorPresenter)
        {
            _behaviorPresenter = GetMonoBehaviorInstansInScene<EnemyBehaviorPresenter>();
        }
        _hpPresenter.Init();
        _behaviorPresenter.Init();
    }

    public void Charge()
    {
        _behaviorPresenter.Charge();
    }

    public bool IsTriggerWeakPoint()
    {
        return _behaviorPresenter.IsTriggerWeakPoint();
    }

    public bool IsChargeTimeUp()
    {
        return _behaviorPresenter.IsEndCharge();
    }

    public async UniTask Attack(IDamage player)
    {
        await _behaviorPresenter.Attack(player);
    }

    public async UniTask Damage()
    {
        _hpPresenter.Damage(1);
        await _behaviorPresenter.Damage();      
    }

    public void Threat()
    {
        _behaviorPresenter?.Threat();
    }

    public void Down()
    {
        _behaviorPresenter?.Down();
    }
    private T GetMonoBehaviorInstansInScene<T>() where T : MonoBehaviour
    {
        T instans = GameObject.FindObjectOfType<T>();
        if (instans)
        {
            return instans;
        }
        else
        {
            GameObject go = new GameObject(nameof(T));
            return go.AddComponent<T>();
        }
    }
}
