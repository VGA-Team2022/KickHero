using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManagerView : MonoBehaviour
{
    [SerializeField]
    Slider _bgmSlider;
    [SerializeField]
    Slider _seSlider;
    [SerializeField]
    Slider _voiceSlider;

    public Slider BGMSlider { get => _bgmSlider; }
    public Slider SESlider { get => _seSlider; }
    public Slider VoiceSlider { get => _voiceSlider; }

    public void SetMaxValue()
    {
        _bgmSlider.maxValue = 1;
        _seSlider.maxValue = 1;
        _voiceSlider.maxValue = 1;
    }
}
