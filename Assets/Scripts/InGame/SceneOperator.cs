using UnityEngine;
using System.Threading;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class SceneOperator
{
    /// <summary>
    /// �V�[���Ԃŕێ����镶����
    /// </summary>
    string _message;

    public SceneOperator(string message)
    {
        SetUp(message);
    }

    public void SetUp(string message)
    {
        _message = message;
    }

    public async UniTask LoadScene(string sceneName)
    {
        //�V�[����j��
        await GetActiveAbstructScene(SceneManager.GetActiveScene()).UnLoad();

        //�V�[�������[�h
        await SceneManager.LoadSceneAsync(sceneName);

        //���[�h���AbstructScene���擾
        var absScene = GetActiveAbstructScene(SceneManager.GetSceneByName(sceneName));
        absScene.SetOperator(this);

        //���[�h���̏������Ă�
        await absScene.Load(_message);
        absScene.Open();
    }

    /// <summary>
    /// �V�[������RootObjects����AbstructScene��Ԃ�
    /// </summary>
    AbstructScene GetActiveAbstructScene(Scene scene)
    {
        AbstructScene abstructScene = null;
        foreach (var obj in scene.GetRootGameObjects())
        {
            if (obj.TryGetComponent(out AbstructScene getAbstructScene))
            {
                abstructScene = getAbstructScene;
                break;
            }
        }
        if (abstructScene)
        {
            return abstructScene;
        }
        else
        {
            throw new System.ArgumentNullException($"{scene.name}��RootObjects��AbstructScene���A�^�b�`���ꂽ�I�u�W�F�N�g���܂܂�Ă��܂���B");
        }
    }
}