using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
	[Header("Mixer Principal")]
	[SerializeField] private AudioMixer mainMixer;

	[Header("Sliders de Volume")]
	[SerializeField] private Slider masterSlider;
	[SerializeField] private Slider musicSlider;
	[SerializeField] private Slider sfxSlider;

	// Define o volume mínimo em decibéis para quando o slider NÃO está em zero.
	// -40f dB é um bom ponto de partida para um som "baixo" mas audível.
	// Quanto menos negativo, menos chance de "sumir" o som cedo demais.
	private const float MIN_LOG_VOLUME_DB = -40f;

	// O volume para quando o slider está EXATAMENTE em zero.
	private const float MUTE_VOLUME_DB = -80f; // Padrão da Unity para mudo absoluto

	void Start()
	{
		// Carrega os valores salvos. Se não houver valor, usa 1.0f (volume máximo).
		masterSlider.value = PlayerPrefs.GetFloat("Master", 1.0f);
		musicSlider.value = PlayerPrefs.GetFloat("Music", 1.0f);
		sfxSlider.value = PlayerPrefs.GetFloat("SFX", 1.0f);

		// Aplica os valores iniciais ao Audio Mixer.
		SetMasterVolume(masterSlider.value);
		SetMusicVolume(musicSlider.value);
		SetSFXVolume(sfxSlider.value);
	}

	// Função para o Volume Mestre
	public void SetMasterVolume(float sliderValue)
	{
		float volumeInDb;

		if (sliderValue == 0f) // Se o slider estiver EXATAMENTE no zero
		{
			volumeInDb = MUTE_VOLUME_DB; // Mudo absoluto
		}
		else
		{
			// Converte o valor linear do slider (0-1) para uma escala de decibéis (logarítmica).
			// O 0.0001f evita Math.Log10(0), que resultaria em infinito negativo.
			float linearValueForLog = Mathf.Max(sliderValue, 0.0001f);
			volumeInDb = Mathf.Log10(linearValueForLog) * 20;

			// Garante que o volume não caia abaixo de um certo ponto audível,
			// a menos que o slider esteja em zero.
			volumeInDb = Mathf.Max(volumeInDb, MIN_LOG_VOLUME_DB);
		}

		mainMixer.SetFloat("Master", volumeInDb);
		PlayerPrefs.SetFloat("Master", sliderValue);
	}

	// Funções para Música e SFX (faça as mesmas alterações aqui)
	public void SetMusicVolume(float sliderValue)
	{
		float volumeInDb;
		if (sliderValue == 0f)
		{
			volumeInDb = MUTE_VOLUME_DB;
		}
		else
		{
			float linearValueForLog = Mathf.Max(sliderValue, 0.0001f);
			volumeInDb = Mathf.Log10(linearValueForLog) * 20;
			volumeInDb = Mathf.Max(volumeInDb, MIN_LOG_VOLUME_DB);
		}
		mainMixer.SetFloat("Music", volumeInDb);
		PlayerPrefs.SetFloat("Music", sliderValue);
	}

	public void SetSFXVolume(float sliderValue)
	{
		float volumeInDb;
		if (sliderValue == 0f)
		{
			volumeInDb = MUTE_VOLUME_DB;
		}
		else
		{
			float linearValueForLog = Mathf.Max(sliderValue, 0.0001f);
			volumeInDb = Mathf.Log10(linearValueForLog) * 20;
			volumeInDb = Mathf.Max(volumeInDb, MIN_LOG_VOLUME_DB);
		}
		mainMixer.SetFloat("SFX", volumeInDb);
		PlayerPrefs.SetFloat("SFX", sliderValue);
	}
}