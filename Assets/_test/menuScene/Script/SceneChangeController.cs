using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneChangeController : ButtonBase
{
    [SerializeField,Tooltip("遷移先のシーン名")] private string _sceneName;

    [SerializeField,Tooltip("フェイドさせるキャンバス")] private CanvasGroup _canvasGroup;
    [SerializeField,Tooltip("フェイド用UI")] private Image _fade;
    [SerializeField,Tooltip("フェイドしきるまでの時間")] private float _fadeTime = 0.5f;

    public override void Process()
    {
        base.Process();
        _canvasGroup.DOFade(1, _fadeTime)
            .OnStart(() =>
            {
                _fade.enabled = true;
                SoundManagerPresenter.Instance.CriAtomSEPlay("SE_Pressed_2");
            })
            .OnComplete(() => 
            {
                SceneManager.LoadScene(_sceneName);
            }) ;
    }
}
