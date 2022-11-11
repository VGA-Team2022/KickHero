using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class AbstructScene : MonoBehaviour
{
    protected SceneOperator _sceneOperator;
    /// <summary>
    /// 現在クリアしたステージの配列
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
    /// SceneOperatorのLoadSceneを非同期で呼ぶ
    /// </summary>
    public async void LoadScene(string sceneName,bool[] clearedStage)
    {
        //最初に一度インスタンスを初期化
        if (_sceneOperator == null)
        {
            _sceneOperator = new SceneOperator(clearedStage);
        }
        await _sceneOperator.LoadScene(sceneName);
    }

    /// <summary>
    /// 前のシーンからSceneOperatorを引き継ぐための関数
    /// </summary>
    public void SetOperator(SceneOperator appOperator)
    {
        _sceneOperator = appOperator;
    }
    
    /// <summary>
    /// ロード時に呼ばれる
    /// </summary>
    public abstract UniTask Load();
    /// <summary>
    /// ロードされた後に呼ばれる
    /// </summary>
    public abstract void Open();
    /// <summary>
    /// シーンの破棄時に呼ばれる
    /// </summary>
    public abstract UniTask UnLoad();
}
