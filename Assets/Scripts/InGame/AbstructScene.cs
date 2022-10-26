using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class AbstructScene : MonoBehaviour
{
    protected SceneOperator _sceneOperator;

    private void Awake()
    {
        OnAwake();
    }
    protected virtual void OnAwake() { }

    /// <summary>
    /// SceneOperator��LoadScene��񓯊��ŌĂ�
    /// </summary>
    public async void LoadScene(string sceneName,string message)
    {
        //�ŏ��Ɉ�x�C���X�^���X��������
        if (_sceneOperator == null)
        {
            _sceneOperator = new SceneOperator(message);
        }
        await _sceneOperator.LoadScene(sceneName);
    }

    /// <summary>
    /// �O�̃V�[������SceneOperator�������p�����߂̊֐�
    /// </summary>
    public void SetOperator(SceneOperator appOperator)
    {
        _sceneOperator = appOperator;
    }
    
    /// <summary>
    /// ���[�h���ɌĂ΂��
    /// </summary>
    public abstract UniTask Load(string message);
    /// <summary>
    /// ���[�h���ꂽ��ɌĂ΂��
    /// </summary>
    public abstract void Open();
    /// <summary>
    /// �V�[���̔j�����ɌĂ΂��
    /// </summary>
    public abstract UniTask UnLoad();
}
