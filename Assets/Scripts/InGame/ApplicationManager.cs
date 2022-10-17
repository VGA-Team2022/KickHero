using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    ApplicationOperator _applicationOperator;
    private void Awake()
    {
        _applicationOperator = new ApplicationOperator();
    }
}
