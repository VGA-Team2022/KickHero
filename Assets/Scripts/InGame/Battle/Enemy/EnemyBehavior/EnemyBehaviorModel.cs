using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

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

    public async UniTask Charge()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_chargeTime));
    }

    public void Attack(IDamage damage)
    {
        damage.Damage(-_attack);
    }

    public async UniTask Stan()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_stanTime));
    }
}
