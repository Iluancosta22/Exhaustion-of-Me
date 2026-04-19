using UnityEngine;
using UnityEngine.UI;

public class MouseSetting : MonoBehaviour
{
    public Slider SensitivitySlider;

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
    }
}
