using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

/// <summary>
/// Clase a travñes de la cual se modificarán los ajustes de sonido.
/// </summary>
public class AudioOptions : MonoBehaviour
{
    /// <summary>
    /// El slider del volumen de la música.
    /// </summary>
    [SerializeField] Slider musicSlider = null;
    /// <summary>
    /// El slider del volumen de los SFX.
    /// </summary>
    [SerializeField] Slider sfxSlider = null;
    /// <summary>
    /// AudioMixer del volumen de la música.
    /// </summary>
    [SerializeField] AudioMixerGroup musicMixer = null;
    /// <summary>
    /// AudioMixer del volumen de los SFX.
    /// </summary>
    [SerializeField] AudioMixerGroup sfxMixer = null;
    /// <summary>
    /// El volumen de la música.
    /// </summary>
    float musicVolume;
    /// <summary>
    /// El volumen de los SFX.
    /// </summary>
    float sfxVolume;

    private void Start()
    {
        LoadOptions();
    }

    private void Update()
    {
        musicVolume = musicSlider.value;
        sfxVolume = sfxSlider.value;

        musicMixer.audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 25);
        sfxMixer.audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 25);
    }

    /// <summary>
    /// Función que guarda los ajustes de sonido.
    /// </summary>
    public void SaveOptions()
    {
        SaveManager.saveManager.musicVolume = musicVolume;
        SaveManager.saveManager.sfxVolume = sfxVolume;
        SaveManager.saveManager.SaveOptions();
    }

    /// <summary>
    /// Función que carga los ajustes de sonido.
    /// </summary>
    void LoadOptions()
    {
        float musicVolumeLoaded = SaveManager.saveManager.musicVolume;
        musicSlider.value = musicVolumeLoaded;

        float sfxVolumeLoaded = SaveManager.saveManager.sfxVolume;
        sfxSlider.value = sfxVolumeLoaded;
    }
}