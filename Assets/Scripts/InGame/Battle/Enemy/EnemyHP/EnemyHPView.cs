using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// �G�̕\���Ɋւ��ẴX�N���v�g
/// </summary>
public class EnemyHPView : MonoBehaviour
{
    [SerializeField]
    Slider _slider = null;
    [SerializeField]
    float _sliderDuration = 1f;
    /// <summary>
    /// HP�X���C�_�[�̕\���ύX�p�֐�
    /// </summary>
    /// <param name="maxHp">�ő�HP</param>
    /// <param name="currentHp">�ύX���HP</param>
    public void ChangeSliderValue(int maxHp, int currentHp)
    {
        _slider.maxValue = maxHp;
        UIAnimationUtil.GaugeAnimation(_slider,_slider.value,currentHp,_sliderDuration);
    }
}
