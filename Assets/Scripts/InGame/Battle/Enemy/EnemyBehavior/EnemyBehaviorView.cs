using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(Animator))]
public class EnemyBehaviorView : MonoBehaviour
{
    Animator _animator;
    ObservableStateMachineTrigger _trigger;
    [SerializeField]
    GameObject _deathPrefab;

    [SerializeField]
    GameObject _weakPointUI;

    [SerializeField]
    WeakPoint[] _weakPoints;

    public void Init()
    {
        _animator = GetComponent<Animator>();
        _trigger = _animator.GetBehaviour<ObservableStateMachineTrigger>();
        _weakPointUI.SetActive(false);
        
    }

    public async UniTask PlayAttackAnimation()
    {
        _animator.SetTrigger(AnimationName.EnemyAnimationNames.Attack);
        await _trigger.OnStateExitAsObservable()
            .Where(onStateInfo => onStateInfo.StateInfo.IsName(AnimationName.EnemyAnimationNames.Attack))
            .ToUniTask(true);
    }

    public void PlayChargeAnimation()
    {
        _animator.SetTrigger(AnimationName.EnemyAnimationNames.Charge);
    }

    public async UniTask PlayDamageAnimation()
    {
        _animator.SetTrigger(AnimationName.EnemyAnimationNames.HitRight);
        await _trigger.OnStateExitAsObservable()
            .Where(onStateInfo=>onStateInfo.StateInfo.IsName(AnimationName.EnemyAnimationNames.HitRight))
            .ToUniTask(true);
    }
    
    public void PlayStanAnimation(bool value)
    {
        _animator.SetTrigger(AnimationName.EnemyAnimationNames.Down);
    }

    public void ActiveWeakPoint(bool isActive)
    {
        _weakPointUI.SetActive(isActive);
        if (!isActive)
        {
            foreach (var weakpoint in _weakPoints)
            {
                weakpoint.IsTrigger = false;
            }
            return;
        }
        int rand = UnityEngine.Random.Range(0, _weakPoints.Length);
        _weakPointUI.transform.position = Camera.main.WorldToScreenPoint(_weakPoints[rand].transform.position);
    }

    public bool IsTriggerWeakPoint()
    {
        bool isPrevent = false;
        foreach (var weakpoint in _weakPoints)
        {
            if (weakpoint.IsTrigger)
            {
                isPrevent = weakpoint.IsTrigger;
                break;
            }
        }
        return isPrevent;
    }

    public void ResetWeakPoints()
    {
        foreach (var weakpoint in _weakPoints)
        {
            weakpoint.IsTrigger = false;
            weakpoint.gameObject.SetActive(false);
        }
    }

    public void Down()
    {
        Instantiate(_deathPrefab);
        Destroy(this.gameObject);
    }
}
