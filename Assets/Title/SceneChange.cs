using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{
    [SerializeField] GameObject _panel;

    Image _panelImage;

    float _alphaData;

    float _fadeDate = 0.001f;

    bool _fadeFrag = false;

    // Start is called before the first frame update
    void Start()
    {
        _panelImage = _panel.GetComponent<Image>();
        _alphaData = _panelImage.color.a;
        Debug.Log("�ŏ��̃��l��" + _alphaData);
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

    public void Isfade()
    {
        _fadeFrag = true;
    }

    void PlayGmaim()
    {
        //�X�e�[�W�Z���N�g�V�[���ֈړ�
        SceneManager.CreateScene("");
    }

    void Fade()
    {
        _panel.SetActive(true);
        _alphaData += _fadeDate;
        _panelImage.color = new(0, 0, 0, _alphaData);
        Debug.Log("���̃��l��" + _alphaData);

        if (_alphaData > 1)
        {
            Debug.Log("�������Ă���");
            _fadeFrag = false;
            PlayGmaim();
        }
    }
}
