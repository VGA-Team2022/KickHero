using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;

public class ButtonBase : MonoBehaviour, IPointerClickHandler
{
    [SerializeField,Tooltip("アニメーションの全体時間")] private float _duration = 0.1f;

    private bool _onClick = false;   //押下時かどうかを管理

    public async void OnPointerClick(PointerEventData eventData)
    {
        if (!_onClick)  //ボタン連打を防ぐ条件分岐
        {
            _onClick = await UniTaskAnimation(this.GetCancellationTokenOnDestroy());
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
    /// ボタンを押した時の処理を書く
    /// </summary>
    public virtual void Process() { }
}
