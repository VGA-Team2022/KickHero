using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageStartButton : MonoBehaviour
{
    Button _button;
    string _sceneName;
    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(async () =>
        {
            var menuScene = FindObjectOfType<MenuScene>();
            await menuScene.LoadScene(_sceneName);
        });
    }

    public void SetSceneName(string name)
    {
        _sceneName = name;
    }
}
