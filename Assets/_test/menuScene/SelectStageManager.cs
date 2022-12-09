using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectStageManager : MonoBehaviour
{
    [SerializeField,Tooltip("データの保管場所")] KeepData _keepData;

    [SerializeField, Tooltip("押下時にステージ情報のスクリーン表示")]
    private Button[] _stageButtons;
    [SerializeField, Tooltip("ステージ情報スクリーン")]
    private GameObject[] _informationPanels;
    [SerializeField, Tooltip("押下時にステージ情報スクリーンを閉じる")]
    private Button[] _returnSelectStages;
    [SerializeField, Tooltip("押下時にステージシーンをロード")]
    private Button[] _loadScenes;
    [SerializeField, Tooltip("シーン名")]
    private string[] _sceneNames;


    private void Awake()
    {
        //保存してるデータの数値に等しいインデックス番目のボタンを押せるようにする
        int num = _keepData._countClear;
        if (_keepData._countClear >= _stageButtons.Length)
        {
            num = _stageButtons.Length - 1;
        }
        for (int i = 1; i <= num; i++)
        {
            _stageButtons[i].interactable = true;
        }
    }

    /// <summary>
    /// ステージ情報スクリーンを開く
    /// </summary>
    /// <param name="index">インデックス</param>
    public void OnOpenScreen(int index)
    {
        for(int i = 0; i < _stageButtons.Length; i++)
        {
            if (_stageButtons[index] == _stageButtons[i])
            {
                _informationPanels[i].SetActive(true);
                break;
            }
        }
    }

    /// <summary>
    /// ステージ情報スクリーンを閉じる
    /// </summary>
    /// <param name="index">インデックス</param>
    public void OnReturnSelectStage(int index)
    {
        for(int i = 0; i < _returnSelectStages.Length; i++)
        {
            if (_returnSelectStages[index] == _returnSelectStages[i])
            {
                _informationPanels[i].SetActive(false);
                break;
            }
        }
    }

    /// <summary>
    /// ステージシーンをロード
    /// </summary>
    /// <param name="index">インデックス</param>
    public void OnLoadScene(int index)
    {
        for(int i = 0; i < _loadScenes.Length; i++)
        {
            if (_loadScenes[index] == _loadScenes[i])
            {
                SceneManager.LoadScene(_sceneNames[i]);
                break;
            }
        }
    }
}
