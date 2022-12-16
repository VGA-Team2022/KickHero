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

    public void Init()
    {
        _animator = GetComponent<Animator>();
        _trigger = _animator.GetBehaviour<ObservableStateMachineTrigger>();
    }

    public async UniTask PlayAttackAnimation()
    {
        _animator.SetTrigger(AnimationName.EnemyAnimationNames.Attack);
        Debug.Log("アニメーションはじまり");
        await _trigger.OnStateExitAsObservable().ToUniTask(true);
        Debug.Log("アニメーション終わり");
    }

    public void PlayChargeAnimation()
    {
        _animator.SetTrigger(AnimationName.EnemyAnimationNames.Charge);
    }

    public void PlayDamageAnimation()
    {
        _animator.SetTrigger(AnimationName.EnemyAnimationNames.HitRight);
    }

    public void PlayStanAnimation(bool value)
    {
        _animator.SetTrigger(AnimationName.EnemyAnimationNames.Down);
    }

    public void Down()
    {
        Instantiate(_deathPrefab);
        Destroy(this.gameObject);
    }
}
