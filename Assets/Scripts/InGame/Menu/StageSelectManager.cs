using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectManager : MonoBehaviour
{
    [SerializeField]
    StageSelectButton[] _selectButtons;

    MenuScene _menuScene;
    private void Start()
    {
        _menuScene = FindObjectOfType<MenuScene>();
        _selectButtons[0].SetActicveButton(true);
        for (int i =1;i<  _menuScene.ClearedStages.Length;i++)
        {
            _selectButtons[i].SetActicveButton(_menuScene.ClearedStages[i-1]);
        }
    }
}
