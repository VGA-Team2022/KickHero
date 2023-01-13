using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectButton : MonoBehaviour
{
    [SerializeField]
    Button _button;
    [SerializeField]
    Image _image;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }
    public void SetActicveButton(bool isActive)
    {
        _button.interactable = isActive;
        if (isActive)
        {
            _image.color = Color.white;          
        }
        else
        {
            _image.color = Color.gray;
        }
    }
}
