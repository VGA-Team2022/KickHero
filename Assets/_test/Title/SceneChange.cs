using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{
    /// <summary>フェードアウト用のパネル</summary>
    [SerializeField] GameObject _panel;

    /// <summary>フェードアウト用のパネルのイメージを取得するための変数</summary>
    Image _panelImage;

    /// <summary>イメージのα値を取得するための変数</summary>
    float _alphaData;
    /// <summary>フェードアウトする時の感覚</summary>
    [SerializeField] float _fadeDate = 0.001f;

    /// <summary>フェードアウトするかどうか判定するフラグ</summary>
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

    //スタートボタンを押したときにフェードを行うフラグを変更する関数
    public void Isfade()
    {
        _fadeFrag = true;
    }

    //フェードアウトを行った後にシーンを切り替える関数
    void Fade()
    {
        _panel.SetActive(true);
        _alphaData += _fadeDate;
        _panelImage.color = new(0, 0, 0, _alphaData);

        if (_alphaData > 1)
        {
            _fadeFrag = false;
            //ステージセレクトシーンへ移動
            SceneManager.CreateScene("");
        }
    }
}
