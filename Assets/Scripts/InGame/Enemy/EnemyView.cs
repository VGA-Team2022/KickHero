using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 敵の表示に関してのスクリプト
/// </summary>
public class EnemyView : MonoBehaviour
{

    [SerializeField] Slider _slider = null;

    /// <summary>
    /// HPスライダーの表示変更用関数
    /// </summary>
    /// <param name="maxHp">最大HP</param>
    /// <param name="currentHp">変更後のHP</param>
    public void ChangeSliderValue(int maxHp,int currentHp)
    {
        _slider.maxValue = maxHp;
        _slider.value = currentHp;
    }

    /// <summary>
    /// 攻撃の時に発生するモーションを呼ぶ関数
    /// </summary>
    public void NormalAttackMove()
    {

    }

    /// <summary>
    /// 大技攻撃のモーションを呼ぶ関数
    /// </summary>
    public void SpecialAttackMove()
    {

    }

    /// <summary>
    /// ダメージを受けたときに発生するモーションを呼ぶ関数
    /// </summary>
    public void DamageMove()
    {

    }

    /// <summary>
    /// 倒された時に発生するモーションを呼ぶ関数
    /// </summary>
    public void DeathMove()
    {

    }
}
