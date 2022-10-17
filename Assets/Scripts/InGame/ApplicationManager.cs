using Cysharp.Threading.Tasks;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    ApplicationOperator _applicationOperator;
    async private void Awake()
    {
        _applicationOperator = new ApplicationOperator();
        var so = _applicationOperator.SetUp();
        await so.LoadScene("InGame");
    }
}
