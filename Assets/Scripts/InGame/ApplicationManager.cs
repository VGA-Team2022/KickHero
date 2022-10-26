using Cysharp.Threading.Tasks;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    ApplicationOperator _applicationOperator;
    [SerializeField]
    UnityEngine.UI.Button _button;
    [SerializeField]
    string _sceneName = "Menu";
    private void Awake()
    {
        _button.onClick.AddListener(() => { LoadScene(_sceneName); });
    }

    public async void LoadScene(string sceneName)
    {
        if (_applicationOperator==null)
        {
            _applicationOperator = new ApplicationOperator();
        }
        await _applicationOperator.LoadScene(sceneName);
    }
}
