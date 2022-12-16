using System.Collections;
using System.Collections.Generic;
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

    public async UniTask<bool> Charge()
    {
       return  await  _behaviorPresenter.Charge();
    }

    public async UniTask Attack(IDamage player)
    {
        await _behaviorPresenter.Attack(player);
    }

    public async UniTask Damage()
    {
       await _behaviorPresenter.Damage();
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
