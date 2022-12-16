using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyBehaviorView : MonoBehaviour
{
    Animator _animator;
    [SerializeField]
    GameObject _deathPrefab;

    public void Init()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayAttackAnimation()
    {
        _animator.SetTrigger(AnimationName.EnemyAnimationNames.Attack);
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
