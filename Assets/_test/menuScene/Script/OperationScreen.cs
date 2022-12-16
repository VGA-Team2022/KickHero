using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OperationScreen : ButtonBase
{
    [SerializeField,Tooltip("スクリーンを開いているかどうかの判定")] private bool _isOpen;
    [SerializeField,Tooltip("スクリーンの親")] private GameObject _panel;
    [SerializeField,Tooltip("DoTweenのアニメーションにかかる時間")] private float _time = 0.1f;

    public override void Process()
    {
        base.Process();
        var rectTransform = _panel.GetComponent<RectTransform>();
        if (!_isOpen) //スクリーンを開いていないとき
        {
            rectTransform.anchoredPosition = new Vector2(Screen.width, 0);
            rectTransform.DOAnchorPosX(0, _time).OnStart(() => _panel.SetActive(true)); //画面外から左方向に画面中央に向かう

        }
        else //スクリーンを開いているとき
        {
            rectTransform.DOAnchorPosX(Screen.width,_time).OnComplete(() => _panel.SetActive(false)); //画面中央から右方向に画面外へ出ていく
        }
    }
}
