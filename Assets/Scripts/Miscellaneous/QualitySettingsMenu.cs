using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QualitySettingsMenu : MonoBehaviour
{
    [Header("Dropdowns")]
    public TMP_Dropdown resolutionDropdown;

    [Header("Toggles")]
    public Toggle FullScreen;
    public Toggle Vsync;

    private List<Resolution> validResolutions;

    void Start()
    {
        if (resolutionDropdown == null) return;

        Vsync.isOn = PlayerPrefs.GetInt("VsyncSetting", 1) == 1;
        FullScreen.isOn = PlayerPrefs.GetInt("FullscreenSetting", 1) == 1;
    
        SetupResolutions();
    }

    void SetupResolutions()
    {
        resolutionDropdown.ClearOptions();

        // Pega todas as resoluções suportadas
        var allResolutions = Screen.resolutions;

        // --- FILTRAGEM ---
        validResolutions = allResolutions
            .GroupBy(r => new { r.width, r.height })
            .Select(g => g.First())
            .Where(r =>
                r.width >= 1280 &&
                (Mathf.Abs((float)r.width / r.height - 16f / 9f) < 0.05f ||
                 Mathf.Abs((float)r.width / r.height - 16f / 10f) < 0.05f))
            .OrderByDescending(r => r.width)
            .ToList();

        if (validResolutions.Count == 0)
        {
            validResolutions.Add(new Resolution { width = 1920, height = 1080});
        }

        // Cria opções para o dropdown
        var options = validResolutions
            .Select(r => $"{r.width}x{r.height}")
            .ToList();

        resolutionDropdown.AddOptions(options);

        // Verifica se é a primeira vez que o jogo é aberto
        bool firstLaunch = !PlayerPrefs.HasKey("ResolutionSetting");

        int resolutionIndex;

        if (firstLaunch)
        {
            // Pega a maior resolução disponível (índice 0 pois a lista está em ordem decrescente)
            resolutionIndex = 0;

            // Salva para futuras execuções
            PlayerPrefs.SetInt("ResolutionSetting", resolutionIndex);
            PlayerPrefs.Save();

            Debug.Log("Primeira vez abrindo o jogo — aplicando resolução máxima.");
        }
        else
        {
            // Usa a resolução salva
            resolutionIndex = PlayerPrefs.GetInt("ResolutionSetting", 0);
        }

        resolutionDropdown.value = resolutionIndex;
        resolutionDropdown.RefreshShownValue();

        ApplyResolution(resolutionIndex);
    }

    public void ApplyResolution(int index)
    {
        if (index < 0 || index >= validResolutions.Count)
            return;

        var res = validResolutions[index];
        var currentMode = Screen.fullScreenMode;

        Screen.SetResolution(res.width, res.height, currentMode);

        PlayerPrefs.SetInt("ResolutionSetting", index);
        PlayerPrefs.Save();

        Debug.Log($"Resolução alterada para: {res.width}x{res.height} ({currentMode})");
    }

    public void SetQualityFromDropdown(int index)
    {
        QualitySettings.SetQualityLevel(index);
        PlayerPrefs.SetInt("QualitySetting", index);
        PlayerPrefs.Save();
        Debug.Log("Qualidade alterada para: " + QualitySettings.names[index]);
    }

    public void ToggleFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("FullscreenSetting", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log("Tela cheia: " + isFullscreen);
    }

    public void ToggleVsync(bool enabled)
    {
        QualitySettings.vSyncCount = enabled ? 1 : 0;

        PlayerPrefs.SetInt("VsyncSetting", enabled ? 1 : 0);
        PlayerPrefs.Save();

        Debug.Log("VSync: " + enabled);
    }
    
}
