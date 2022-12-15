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

    public void PlayStanAnimation(bool value)
    {
        _animator.SetBool(AnimationName.EnemyAnimationNames.StanRight,value);
    }

    public void Down()
    {
        Instantiate(_deathPrefab);
        Destroy(this.gameObject);
    }
}
