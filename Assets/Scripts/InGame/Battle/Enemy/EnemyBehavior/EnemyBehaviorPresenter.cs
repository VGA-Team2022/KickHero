using UniRx;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class EnemyBehaviorPresenter : MonoBehaviour
{
    EnemyBehaviorModel _behaviorModel;

    [Header("コンポーネント")]
    [SerializeField]
    EnemyBehaviorView _behaviorView;

    [Header("パラメータ")]
    [SerializeField]
    int _attack = 5;
    [SerializeField]
    float _stanTime = 3f;
    [SerializeField]
    int _enemyHp = 20;
    [SerializeField]
    float _attackTimeLimit = 2f;

    float _timer = 0f;
    public void Init()
    {
        _behaviorModel = new EnemyBehaviorModel(_attack, _stanTime);
        _behaviorView?.Init();
    }

    /// <summary>
    /// 死亡時に呼ばれる関数
    /// </summary>
    public void Down()
    {
        _behaviorView.Down();
    }

    public void Attack(IDamage damage)
    {
        if (_attackTimeLimit > _timer)
        {
            _timer += Time.deltaTime;
        }
        else
        {
            _behaviorView.PlayAttackAnimation();
            _behaviorModel.Attack(damage);
            _timer = 0f;
        }
    }

    public async UniTask Stan()
    {
        _behaviorView.PlayStanAnimation(true);
        await _behaviorModel.Stan();
        _behaviorView.PlayStanAnimation(false);
    }
}
