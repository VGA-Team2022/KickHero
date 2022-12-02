using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// �G�̕\���Ɋւ��ẴX�N���v�g
/// </summary>
public class EnemyHPView : MonoBehaviour
{
    [SerializeField]
    Slider _slider = null;

    /// <summary>
    /// HP�X���C�_�[�̕\���ύX�p�֐�
    /// </summary>
    /// <param name="maxHp">�ő�HP</param>
    /// <param name="currentHp">�ύX���HP</param>
    public void ChangeSliderValue(int maxHp, int currentHp)
    {
        _slider.maxValue = maxHp;
        _slider.value = currentHp;
    }
}
