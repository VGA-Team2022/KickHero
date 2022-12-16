using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Enemy
{
    EnemyHPPresenter _hpPresenter;
    EnemyBehaviorPresenter _behaviorPresenter;

    IDamage _playerDamage;
    public Enemy(System.Action<InGameCycle.EventEnum> action, IDamage playerDamage)
    {
        Initialize(action);
        _playerDamage = playerDamage;
    }
    public void Initialize(System.Action<InGameCycle.EventEnum> action)
    {
        if (!_hpPresenter)
        {
            _hpPresenter = GetMonoBehaviorInstansInScene<EnemyHPPresenter>();
        }
        if (!_behaviorPresenter)
        {
            _behaviorPresenter = GetMonoBehaviorInstansInScene<EnemyBehaviorPresenter>();
        }
        _hpPresenter.Init(action);
        _behaviorPresenter.Init();
    }

    public async UniTask Charge()
    {
        await _behaviorPresenter.Charge();
    }

    public async UniTask Attack(IDamage player)
    {
        await _behaviorPresenter.Attack(player);
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
