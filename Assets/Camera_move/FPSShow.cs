using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSShow : MonoBehaviour
{
    [SerializeField]
    private float m_updateInterval = 0.5f;

    private float m_accum;
    private int m_frames;
    private float m_timeleft;
    private float m_fps;
    private GUIStyle style;
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    void Start()
    {
        style = new GUIStyle();
        style.fontSize = 50;
    }

    private void Update()
    {
        m_timeleft -= Time.deltaTime;
        m_accum += Time.timeScale / Time.deltaTime;
        m_frames++;

        if (0 < m_timeleft) return;

        m_fps = m_accum / m_frames;
        m_timeleft = m_updateInterval;
        m_accum = 0;
        m_frames = 0;
    }

    private void OnGUI()
    {
        Rect rect = new Rect(10, 10, 400, 300);
        GUILayout.Label("FPS: " + m_fps.ToString("f2"), style);
    }
}
