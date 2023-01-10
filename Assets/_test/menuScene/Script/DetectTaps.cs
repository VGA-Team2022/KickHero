using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class DetectTaps : MonoBehaviour,IPointerClickHandler
{
    [SerializeField, Tooltip("�J�ڐ�̃V�[����")] private string _sceneName;

    [SerializeField, Tooltip("�t�F�C�h������L�����o�X")] private CanvasGroup _canvasGroup;
    [SerializeField, Tooltip("�t�F�C�h�pUI")] private Image _fade;
    [SerializeField, Tooltip("�t�F�C�h������܂ł̎���")] private float _fadeTime = 0.5f;

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
