using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    string _sceneName;
    UnityEngine.UI.Button _button;

    private void Start()
    {
        _button = GetComponent<UnityEngine.UI.Button>();
        _button.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(_sceneName);
        });
        SoundManagerPresenter.Instance.CriAtomVoicePlay("Voice_Title_001");
        DOVirtual.DelayedCall(1.5f, () =>
        {
            SoundManagerPresenter.Instance.CriAtomBGMPlay("BGM_Title");
        });
    }
}
