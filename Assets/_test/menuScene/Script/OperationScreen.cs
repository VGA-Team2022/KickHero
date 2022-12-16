using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OperationScreen : ButtonBase
{
    [SerializeField,Tooltip("�X�N���[�����J���Ă��邩�ǂ����̔���")] private bool _isOpen;
    [SerializeField,Tooltip("�X�N���[���̐e")] private GameObject _panel;
    [SerializeField,Tooltip("DoTween�̃A�j���[�V�����ɂ����鎞��")] private float _time = 0.1f;

    public override void Process()
    {
        base.Process();
        var rectTransform = _panel.GetComponent<RectTransform>();
        if (!_isOpen) //�X�N���[�����J���Ă��Ȃ��Ƃ�
        {
            rectTransform.anchoredPosition = new Vector2(Screen.width, 0);
            rectTransform.DOAnchorPosX(0, _time).OnStart(() => _panel.SetActive(true)); //��ʊO���獶�����ɉ�ʒ����Ɍ�����

        }
        else //�X�N���[�����J���Ă���Ƃ�
        {
            rectTransform.DOAnchorPosX(Screen.width,_time).OnComplete(() => _panel.SetActive(false)); //��ʒ�������E�����ɉ�ʊO�֏o�Ă���
        }
    }
}
