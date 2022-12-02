using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 敵の表示に関してのスクリプト
/// </summary>
public class EnemyHPView : MonoBehaviour
{
    [SerializeField]
    Slider _slider = null;

    /// <summary>
    /// HPスライダーの表示変更用関数
    /// </summary>
    /// <param name="maxHp">最大HP</param>
    /// <param name="currentHp">変更後のHP</param>
    public void ChangeSliderValue(int maxHp, int currentHp)
    {
        _slider.maxValue = maxHp;
        _slider.value = currentHp;
    }
}
