using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;

public class ButtonBase : MonoBehaviour, IPointerClickHandler
{
    [SerializeField,Tooltip("�A�j���[�V�����̑S�̎���")] private float _duration = 0.1f;

    Button _button;

    private bool _onClick = false;   //���������ǂ������Ǘ�

    private void Start()
    {
        _button = GetComponent<Button>();
    }


    public async void OnPointerClick(PointerEventData eventData)
    {
        if (_button == null || _button.interactable == true) //�{�^�����A�N�e�B�u�̂Ƃ��A�܂���Button�R���|�[�l���g���Ȃ��Ƃ�
        {
            if (!_onClick)  //�{�^���A�ł�h����������
            {
                _onClick = await UniTaskAnimation(this.GetCancellationTokenOnDestroy());
            }
        }
    }

    async UniTask<bool> UniTaskAnimation(CancellationToken cancellationToken)
    {
        _onClick = true;
        int delay = (int)(_duration * 1000) * 2;
        transform.DOScale(0.8f, _duration).SetLoops(2, LoopType.Yoyo);
        await UniTask.Delay(delay, false, PlayerLoopTiming.Update, cancellationToken);
        Process();
        return false;
    }

    /// <summary>
    /// �{�^�������������̏���������
    /// </summary>
    public virtual void Process() { }
}
