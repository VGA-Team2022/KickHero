using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class DrawLineTimerSlider : MonoBehaviour
{
    [Tooltip("ボールプレゼンター")]
    [SerializeField] LineReader _lineReader;
    [SerializeField] Vector2 _center;

    Slider _slider;
    // Start is called before the first frame update
    void Start()
    {
        _slider = GetComponent<Slider>();
        if (_lineReader)
        {
            _lineReader.OnDrawStart(() =>
            {
                _slider.gameObject.SetActive(true);
            });
            _lineReader.OnDrawEnd(() =>
            {
                _slider.gameObject.SetActive(false);
            });
        }
        _slider.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_lineReader)
        {
            _slider.value = _lineReader.TimeGage;
            _slider.transform.position = Input.mousePosition + (Vector3)_center;
        }
    }
}
