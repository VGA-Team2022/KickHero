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

    [SerializeField]
    Image _stageImage;
    [SerializeField]
    StageStartButton _startButton;

    [SerializeField]
    Sprite _imagePrefab;

    [SerializeField]
    string _sceneName = "InGame";

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

        _button.onClick.AddListener(() =>
        {
            _stageImage.transform.parent.gameObject.SetActive(true);
            _stageImage.sprite = _imagePrefab;
            _startButton.SetSceneName(_sceneName);
        });
    }
}
