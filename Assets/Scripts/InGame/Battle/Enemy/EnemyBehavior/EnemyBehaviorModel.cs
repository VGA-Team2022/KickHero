using System.Threading;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyBehaviorModel
{
    int _attack = 0;
    float _chargeTime = 0f;
    float _stanTime = 0f;
    public EnemyBehaviorModel(int attack,float chargeTime,float stanTime)
    {
        _attack = attack;
        _chargeTime = chargeTime;
        _stanTime = stanTime;
    }

    public void Attack(IDamage damage)
    {
        damage.Damage(-_attack);
    }

    float _timer = 0f;
    public bool Charge()
    {
        _timer += Time.deltaTime;
        if(_timer < _chargeTime)
        {
            return false;
        }
        ResetTimer();
        return true;
    }

    public void ResetTimer()
    {
        _timer = 0f;
    }

    public async UniTask Stan()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_stanTime));
    }
}
