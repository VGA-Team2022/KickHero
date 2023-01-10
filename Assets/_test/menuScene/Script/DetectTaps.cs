using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class DetectTaps : MonoBehaviour,IPointerClickHandler
{
    [SerializeField, Tooltip("遷移先のシーン名")] private string _sceneName;

    [SerializeField, Tooltip("フェイドさせるキャンバス")] private CanvasGroup _canvasGroup;
    [SerializeField, Tooltip("フェイド用UI")] private Image _fade;
    [SerializeField, Tooltip("フェイドしきるまでの時間")] private float _fadeTime = 0.5f;

    public void OnPointerClick(PointerEventData eventData)
    {
        _canvasGroup.DOFade(1, _fadeTime)
            .OnStart(() =>
            {
                _fade.enabled = true;
            })
            .OnComplete(() =>
            {
                SceneManager.LoadScene(_sceneName);
            });
    }
}
