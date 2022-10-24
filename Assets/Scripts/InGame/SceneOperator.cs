using UnityEngine;
using System.Threading;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class SceneOperator
{
    ApplicationOperator _applicationOperator;
    object[] _objectArgs;

    public void SetUp(ApplicationOperator appOperator, object[] args)
    {
        _applicationOperator = appOperator;
        var objs = new object[] { args, this };
        _objectArgs = objs;
    }
    public async UniTask LoadScene(string sceneName)
    {
        Debug.Log(GetActiveAbstructScene(SceneManager.GetActiveScene())._hoge);
        await SceneManager.LoadSceneAsync(sceneName);
        var absScene = GetActiveAbstructScene(SceneManager.GetSceneByName(sceneName));
        Debug.Log(absScene._hoge);
        absScene.SetOperator(_applicationOperator);

        await absScene.Load(_objectArgs);
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
