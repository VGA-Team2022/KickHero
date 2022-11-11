using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace alpha
{
    public class SampleInGameManager : AbstructScene
{
    /// <summary>�V�[�����Ŏg�p���镶����</summary>
    protected string _message = "";

    [Header("�R���|�[�l���g")]
    [SerializeField, Tooltip("�������Ƀ��[�h���Ă΂��{�^��")]
    Button _loadButton;

    [SerializeField, Tooltip("�������ɕϐ����ύX�����{�^��")]
    Button _inputButton;

    [SerializeField, Tooltip("���̃V�[���ɓn���������������͂���t�B�[���h")]
    protected InputField _inputField = null;

    [SerializeField, Tooltip("���ݕێ�����Ă���ϐ���\������Text")]
    protected Text _text;

    [Header("���[�h�֘A")]
    [SerializeField, Tooltip("���[�h��̃V�[����")]
    string _sceneName;

    protected override void OnAwake()
    {
        //�������Ɏw��̃V�[���Ƀ��[�h����B
        _loadButton.onClick.AddListener(() => { LoadScene(_sceneName, _inputField.text); });

        //�������Ɏ��̃V�[���ɓn���������ύX����
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
    /// �V�[�������[�h���ꂽ��ɌĂ΂��
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

