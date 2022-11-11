using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectStageManager : MonoBehaviour
{
    [SerializeField,Tooltip("�f�[�^�̕ۊǏꏊ")] KeepData _keepData;

    [SerializeField, Tooltip("�������ɃX�e�[�W���̃X�N���[���\��")]
    private Button[] _stageButtons;
    [SerializeField, Tooltip("�X�e�[�W���X�N���[��")]
    private GameObject[] _informationPanels;
    [SerializeField, Tooltip("�������ɃX�e�[�W���X�N���[�������")]
    private Button[] _returnSelectStages;
    [SerializeField, Tooltip("�������ɃX�e�[�W�V�[�������[�h")]
    private Button[] _loadScenes;
    [SerializeField, Tooltip("�V�[����")]
    private string[] _sceneNames;


    private void Awake()
    {
        //�ۑ����Ă�f�[�^�̐��l�ɓ������C���f�b�N�X�Ԗڂ̃{�^����������悤�ɂ���
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
    /// �X�e�[�W���X�N���[�����J��
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
    /// �X�e�[�W���X�N���[�������
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
    /// �X�e�[�W�V�[�������[�h
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
