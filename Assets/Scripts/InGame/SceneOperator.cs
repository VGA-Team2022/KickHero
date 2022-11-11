using UnityEngine;
using System.Threading;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class SceneOperator
{
    /// <summary>
    /// シーン間で保持する文字列
    /// </summary>
    bool[] _isClearedStages;

    public bool[] IsClearedStages => _isClearedStages;

    public void ClearStage(int index)
    {
        _isClearedStages[index] = true;
    }

    public void ResetClearedStage()
    {
        for (int i = 0;i<_isClearedStages.Length;i++)
        {
            _isClearedStages[i] = false;
        }
    }

    public SceneOperator(bool[] clearedStages)
    {
        SetUp(clearedStages);
    }

    public void SetUp(bool[] clearedStages)
    {
        _isClearedStages = clearedStages;
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
        await absScene.Load();
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
