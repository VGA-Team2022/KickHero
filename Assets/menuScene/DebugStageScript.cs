using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DebugStageScript : MonoBehaviour
{
    [SerializeField, Tooltip("データの保管場所")] KeepData _keepData;

    [SerializeField, Tooltip("押下時ステージ選択画面をロード")]
    private Button _returnButton;
    [SerializeField, Tooltip("押下時に強制的にクリア判定をだす")]
    private Button _clearButton;
    [SerializeField, Tooltip("ステージ番号")]
    private int _stageNum;

    // Start is called before the first frame update
    void Start()
    {
        var clear = new ClearJudgmentScript();

        _returnButton.onClick.AddListener(() => { SceneManager.LoadScene("menuScene"); });
        _clearButton.onClick.AddListener(() => { clear.CountStage(_keepData, _stageNum); });
    }
}
