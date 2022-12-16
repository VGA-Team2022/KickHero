using UniRx;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class EnemyBehaviorPresenter : MonoBehaviour
{
    EnemyBehaviorModel _behaviorModel;

    [Header("�R���|�[�l���g")]
    [SerializeField]
    EnemyBehaviorView _behaviorView;

    [Header("�p�����[�^")]
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
    /// ���S���ɌĂ΂��֐�
    /// </summary>
    public void Down()
    {
        _behaviorView.Down();
    }

    public async UniTask<bool> Charge()
    {
        _behaviorView.ActiveWeakPoint(true);
        _behaviorView.PlayChargeAnimation();
        await _behaviorModel.Charge();
        bool isTrigger = _behaviorView.IsTriggerWeakPoint();
        _behaviorView.ActiveWeakPoint(false);
        return isTrigger;
    }

    public async UniTask Attack(IDamage damage)
    {     
        await _behaviorView.PlayAttackAnimation();
        _behaviorModel.Attack(damage);
    }

    public async UniTask Damage()
    {
        await _behaviorView.PlayDamageAnimation();
    }

    public async UniTask Stan()
    {
        _behaviorView.PlayStanAnimation(true);
        await _behaviorModel.Stan();
        _behaviorView.PlayStanAnimation(false);
    }
}
