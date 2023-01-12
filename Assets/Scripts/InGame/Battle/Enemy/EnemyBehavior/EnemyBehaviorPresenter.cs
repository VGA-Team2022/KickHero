using UniRx;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

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

    public void Charge()
    {
        _behaviorModel.ResetTimer();
        _behaviorView.ActiveWeakPoint(true);
        _behaviorView.PlayChargeAnimation();       
    }

    public bool IsTriggerWeakPoint()
    {
        return _behaviorView.IsTriggerWeakPoint();
    }

    public bool IsEndCharge()
    {
        if (_behaviorModel.Charge() == false)
        {
            return false;
        }
        _behaviorView.ActiveWeakPoint(false);
        return true;
    }

    public async UniTask Attack(IDamage damage)
    {
        await _behaviorView.PlayAttackAnimation();
        _behaviorModel.Attack(damage);
    }

    public async UniTask Damage()
    {
        _behaviorModel.ResetTimer();
        _behaviorView.ActiveWeakPoint(false);
        await _behaviorView.PlayDamageAnimation();
    }

    public async UniTask Stan()
    {
        _behaviorView.PlayStanAnimation(true);
        await _behaviorModel.Stan();
        _behaviorView.PlayStanAnimation(false);
    }
}
