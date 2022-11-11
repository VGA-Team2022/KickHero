using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class AbstructScene : MonoBehaviour
{
    protected SceneOperator _sceneOperator;
    /// <summary>
    /// ���݃N���A�����X�e�[�W�̔z��
    /// </summary>
    public bool[] _isClearedStages => _sceneOperator.IsClearedStages;

    public void ClearStage(int index)
    {
        _sceneOperator.ClearStage(index);
    }

    public void ResetStage()
    {
        _sceneOperator.ResetClearedStage();
    }

    private void Awake()
    {
        OnAwake();
    }
    protected virtual void OnAwake() { }

    /// <summary>
    /// SceneOperator��LoadScene��񓯊��ŌĂ�
    /// </summary>
    public async void LoadScene(string sceneName,bool[] clearedStage)
    {
        //�ŏ��Ɉ�x�C���X�^���X��������
        if (_sceneOperator == null)
        {
            _sceneOperator = new SceneOperator(clearedStage);
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
    public abstract UniTask Load();
    /// <summary>
    /// ���[�h���ꂽ��ɌĂ΂��
    /// </summary>
    public abstract void Open();
    /// <summary>
    /// �V�[���̔j�����ɌĂ΂��
    /// </summary>
    public abstract UniTask UnLoad();
}
