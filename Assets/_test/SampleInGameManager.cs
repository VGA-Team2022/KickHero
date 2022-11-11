using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace alpha
{
    public class SampleInGameManager : AbstructScene
{
    /// <summary>シーン内で使用する文字列</summary>
    protected string _message = "";

    [Header("コンポーネント")]
    [SerializeField, Tooltip("押下時にロードが呼ばれるボタン")]
    Button _loadButton;

    [SerializeField, Tooltip("押下時に変数が変更されるボタン")]
    Button _inputButton;

    [SerializeField, Tooltip("次のシーンに渡したい文字列を入力するフィールド")]
    protected InputField _inputField = null;

    [SerializeField, Tooltip("現在保持されている変数を表示するText")]
    protected Text _text;

    [Header("ロード関連")]
    [SerializeField, Tooltip("ロード先のシーン名")]
    string _sceneName;

    protected override void OnAwake()
    {
        //押下時に指定のシーンにロードする。
        _loadButton.onClick.AddListener(() => { LoadScene(_sceneName, _inputField.text); });

        //押下時に次のシーンに渡す文字列を変更する
        _inputButton.onClick.AddListener(() => {
            if (_sceneOperator == null)
            {
                _sceneOperator = new SceneOperator(_inputField.text);
            }
            else
            {
                _sceneOperator.SetUp(_inputField.text);
            }
        });
    }
    public override async UniTask Load(string message)
    {
        _message = message;
        await UniTask.Yield();
    }

    /// <summary>
    /// シーンがロードされた後に呼ばれる
    /// </summary>
    public override void Open()
    {
        _text.text = _message;
    }

    public override async UniTask UnLoad()
    {
        await UniTask.Yield();
    }
}
}

