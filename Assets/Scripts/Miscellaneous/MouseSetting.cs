using System;
using UnityEngine;
using UnityEngine.UI;

public class MouseSetting : MonoBehaviour
{
    public Slider SensitivitySlider;
    public static event Action OnMouseSensitivity;

    void Start()
    {
        float sensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 0.2f);

        SensitivitySlider.value = sensitivity;
        SetSensitivity(sensitivity);
    }

    public void SetSensitivity(float value)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", value); 
        PlayerPrefs.Save();

        OnMouseSensitivity?.Invoke();
    }
}
