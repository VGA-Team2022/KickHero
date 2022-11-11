using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DebugStageScript : MonoBehaviour
{
    [SerializeField, Tooltip("�f�[�^�̕ۊǏꏊ")] KeepData _keepData;

    [SerializeField, Tooltip("�������X�e�[�W�I����ʂ����[�h")]
    private Button _returnButton;
    [SerializeField, Tooltip("�������ɋ����I�ɃN���A���������")]
    private Button _clearButton;
    [SerializeField, Tooltip("�X�e�[�W�ԍ�")]
    private int _stageNum;

    // Start is called before the first frame update
    void Start()
    {
        var clear = new ClearJudgmentScript();

        _returnButton.onClick.AddListener(() => { SceneManager.LoadScene("menuScene"); });
        _clearButton.onClick.AddListener(() => { clear.CountStage(_keepData, _stageNum); });
    }
}
