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
    float _chargeTime = 3f;
    [SerializeField]
    float _stanTime = 3f;
    [SerializeField]
    int _enemyHp = 20;
    [SerializeField]
    float _attackTimeLimit = 2f;
    public void Init()
    {
        _behaviorModel = new EnemyBehaviorModel(_attack, _chargeTime, _stanTime);
        _behaviorView?.Init();
    }

    /// <summary>
    /// 死亡時に呼ばれる関数
    /// </summary>
    public void Down()
    {
        _behaviorView.Down();
    }

    public async UniTask Charge()
    {
        _behaviorView.PlayChargeAnimation();
        await _behaviorModel.Charge();
    }

    public async UniTask Attack(IDamage damage)
    {     
        await _behaviorView.PlayAttackAnimation();
        _behaviorModel.Attack(damage);
    }

    public async UniTask Stan()
    {
        _behaviorView.PlayStanAnimation(true);
        await _behaviorModel.Stan();
        _behaviorView.PlayStanAnimation(false);
    }
}
