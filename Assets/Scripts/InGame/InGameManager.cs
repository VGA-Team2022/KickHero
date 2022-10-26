using Cysharp.Threading.Tasks;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InGameManager : AbstructScene
{
    [SerializeField]
    UnityEngine.UI.Button _loadButton;
    [SerializeField]
    UnityEngine.UI.Button _inputButton;
    [SerializeField]
    protected UnityEngine.UI.Text _text;
    [SerializeField]
    string _sceneName;
    [SerializeField]
    protected UnityEngine.UI.InputField _inputField = null;

    [SerializeField]
    protected string _hoge = "";

    protected override void OnAwake()
    {
        _loadButton.onClick.AddListener(() => { LoadScene(_sceneName, _inputField.text); });
        _inputButton.onClick.AddListener(() => {
            if (_sceneOperator == null)
            {
                _sceneOperator = new SceneOperator(_inputField.text);
            }
            _sceneOperator.SetUp(_inputField.text);
        });
    }
    public override async UniTask Load(string message)
    {
        _hoge = message;       
        Debug.Log($"LoadíÜ");
        await UniTask.DelayFrame(0);
    }

    public override void Open()
    {
        _text.text = _hoge;
        Debug.Log("Open");
    }

    public override async UniTask UnLoad()
    {
        Debug.Log($"ÉAÉìÉçÅ[Éh{UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
        await UniTask.Yield();
    }
}
