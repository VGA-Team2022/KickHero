using UnityEngine;
using System.Threading;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class SceneOperator
{
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
        Debug.Log(GetActiveAbstructScene(SceneManager.GetActiveScene()).name);
        await SceneManager.LoadSceneAsync(sceneName);
        var absScene = GetActiveAbstructScene(SceneManager.GetSceneByName(sceneName));
        Debug.Log(SceneManager.GetActiveScene().name);
        absScene.SetOperator(this);
        await absScene.Load(_message);
        absScene.Open();
    }

    AbstructScene GetActiveAbstructScene(Scene scene)
    {
        AbstructScene abstructScene = null;
        foreach (var obj in scene.GetRootGameObjects())
        {
            if (obj.TryGetComponent(out AbstructScene getAbstructScene))
            {
                abstructScene = getAbstructScene;
            }
        }
        if (abstructScene)
        {
            return abstructScene;
        }
        else
        {
            throw new System.ArgumentNullException($"{scene.name}のRootObjectsにAbstructSceneがアタッチされたオブジェクトが含まれていません。");
        }
    }
}
