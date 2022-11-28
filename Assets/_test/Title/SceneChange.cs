using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{
    /// <summary>�t�F�[�h�A�E�g�p�̃p�l��</summary>
    [SerializeField] GameObject _panel;

    /// <summary>�t�F�[�h�A�E�g�p�̃p�l���̃C���[�W���擾���邽�߂̕ϐ�</summary>
    Image _panelImage;

    /// <summary>�C���[�W�̃��l���擾���邽�߂̕ϐ�</summary>
    float _alphaData;
    /// <summary>�t�F�[�h�A�E�g���鎞�̊��o</summary>
    [SerializeField] float _fadeDate = 0.001f;

    /// <summary>�t�F�[�h�A�E�g���邩�ǂ������肷��t���O</summary>
    bool _fadeFrag = false;

    // Start is called before the first frame update
    void Start()
    {
        _panelImage = _panel.GetComponent<Image>();
        _alphaData = _panelImage.color.a;
        _panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(_fadeFrag == true)
        {
            Fade();
        }
    }

    //�X�^�[�g�{�^�����������Ƃ��Ƀt�F�[�h���s���t���O��ύX����֐�
    public void Isfade()
    {
        _fadeFrag = true;
    }

    //�t�F�[�h�A�E�g���s������ɃV�[����؂�ւ���֐�
    void Fade()
    {
        _panel.SetActive(true);
        _alphaData += _fadeDate;
        _panelImage.color = new(0, 0, 0, _alphaData);

        if (_alphaData > 1)
        {
            _fadeFrag = false;
            //�X�e�[�W�Z���N�g�V�[���ֈړ�
            SceneManager.CreateScene("");
        }
    }
}
