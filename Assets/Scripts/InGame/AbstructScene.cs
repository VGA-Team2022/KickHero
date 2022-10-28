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
    /// SceneOperatorのLoadSceneを非同期で呼ぶ
    /// </summary>
    public async void LoadScene(string sceneName,string message)
    {
        //最初に一度インスタンスを初期化
        if (_sceneOperator == null)
        {
            _sceneOperator = new SceneOperator(message);
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
    public abstract UniTask Load(string message);
    /// <summary>
    /// ロードされた後に呼ばれる
    /// </summary>
    public abstract void Open();
    /// <summary>
    /// シーンの破棄時に呼ばれる
    /// </summary>
    public abstract UniTask UnLoad();
}
