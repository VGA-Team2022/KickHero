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

    // Start is called before the first frame update
    void Start()
    {
        OnInformationPanel();
        ReturnSelectStage();
        LoadScene();
    }

    /// <summary>
    /// ステージ情報スクリーンを開く
    /// </summary>
    private void OnInformationPanel()
    {
        _stageButtons[0].onClick.AddListener(() => { _informationPanels[0].SetActive(true); });
        _stageButtons[1].onClick.AddListener(() => { _informationPanels[1].SetActive(true); });
        _stageButtons[2].onClick.AddListener(() => { _informationPanels[2].SetActive(true); });
        _stageButtons[3].onClick.AddListener(() => { _informationPanels[3].SetActive(true); });
        _stageButtons[4].onClick.AddListener(() => { _informationPanels[4].SetActive(true); });
        _stageButtons[5].onClick.AddListener(() => { _informationPanels[5].SetActive(true); });
        _stageButtons[6].onClick.AddListener(() => { _informationPanels[6].SetActive(true); });
    }

    /// <summary>
    /// ステージ情報スクリーンを閉じる
    /// </summary>
    private void ReturnSelectStage()
    {
        _returnSelectStages[0].onClick.AddListener(() => { _informationPanels[0].SetActive(false); });
        _returnSelectStages[1].onClick.AddListener(() => { _informationPanels[1].SetActive(false); });
        _returnSelectStages[2].onClick.AddListener(() => { _informationPanels[2].SetActive(false); });
        _returnSelectStages[3].onClick.AddListener(() => { _informationPanels[3].SetActive(false); });
        _returnSelectStages[4].onClick.AddListener(() => { _informationPanels[4].SetActive(false); });
        _returnSelectStages[5].onClick.AddListener(() => { _informationPanels[5].SetActive(false); });
        _returnSelectStages[6].onClick.AddListener(() => { _informationPanels[6].SetActive(false); });
    }

    /// <summary>
    /// ステージシーンをロード
    /// </summary>
    private void LoadScene()
    {
        _loadScenes[0].onClick.AddListener(() => { SceneManager.LoadScene(_sceneNames[0]); });
        _loadScenes[1].onClick.AddListener(() => { SceneManager.LoadScene(_sceneNames[1]); });
        _loadScenes[2].onClick.AddListener(() => { SceneManager.LoadScene(_sceneNames[2]); });
        _loadScenes[3].onClick.AddListener(() => { SceneManager.LoadScene(_sceneNames[3]); });
        _loadScenes[4].onClick.AddListener(() => { SceneManager.LoadScene(_sceneNames[4]); });
        _loadScenes[5].onClick.AddListener(() => { SceneManager.LoadScene(_sceneNames[5]); });
        _loadScenes[6].onClick.AddListener(() => { SceneManager.LoadScene(_sceneNames[6]); });
    }
}
