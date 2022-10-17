using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationOperator
{
    SceneOperator _sceneOperator;
    public ApplicationOperator()
    {
        var objs = new object[] {"Hello World"};
        GameObject go = new GameObject("SceneOperator");
        go.AddComponent<SceneOperator>().SetUp(this, objs);
    }

    public SceneOperator SetUp()
    {
        var objs = new object[] { "Hello World" };
        GameObject go = new GameObject("SceneOperator");
        var so = go.AddComponent<SceneOperator>();
        so.SetUp(this, objs);
        return so;
    }
}
