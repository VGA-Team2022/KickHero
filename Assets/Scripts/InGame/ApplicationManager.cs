using Cysharp.Threading.Tasks;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    ApplicationOperator _applicationOperator;
    private void Awake()
    {
        _applicationOperator = new ApplicationOperator();
         
    }
    async private void Start()
    {
        await _applicationOperator.SetUp();
    }
}
