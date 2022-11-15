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

    /// <summary>
    /// �X�e�[�W���X�N���[�����J��
    /// </summary>
    /// <param name="index">�C���f�b�N�X</param>
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
    /// �X�e�[�W���X�N���[�������
    /// </summary>
    /// <param name="index">�C���f�b�N�X</param>
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
    /// �X�e�[�W�V�[�������[�h
    /// </summary>
    /// <param name="index">�C���f�b�N�X</param>
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
