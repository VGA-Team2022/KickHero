using UnityEngine;
using System.Threading;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class SceneOperator
{
    /// <summary>
    /// シーン間で保持する文字列
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
        //シーンを破棄
        await GetActiveAbstructScene(SceneManager.GetActiveScene()).UnLoad();

        //シーンをロード
        await SceneManager.LoadSceneAsync(sceneName);

        //ロード先のAbstructSceneを取得
        var absScene = GetActiveAbstructScene(SceneManager.GetSceneByName(sceneName));
        absScene.SetOperator(this);

        //ロード時の処理を呼ぶ
        await absScene.Load(_message);
        absScene.Open();
    }

    /// <summary>
    /// シーン内のRootObjectsからAbstructSceneを返す
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
            throw new System.ArgumentNullException($"{scene.name}のRootObjectsにAbstructSceneがアタッチされたオブジェクトが含まれていません。");
        }
    }
}
