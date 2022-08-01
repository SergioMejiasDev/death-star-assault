﻿using System.IO;
using UnityEngine;

/// <summary>
/// Clase que se encarga de guardar los ajuestes en el dispositivo.
/// </summary>
public class SaveManager : MonoBehaviour
{
    public static SaveManager saveManager;

    [Header("Options")]
    public string activeLanguage = "EN";
    public bool firstTime = false;
    public float musicVolume = 1;
    public float sfxVolume = 1;

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("SaveManager");

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        else
        {
            saveManager = this;

            DontDestroyOnLoad(gameObject);

            LoadOptions();
        }
    }

    /// <summary>
    /// Función que guarda las variables de las opciones.
    /// </summary>
    public void LoadOptions()
    {
        OptionsData data = new OptionsData();

        string json;

        string path = Application.persistentDataPath + "/Options.json";

        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                json = reader.ReadToEnd();
            }

            JsonUtility.FromJsonOverwrite(json, data);

            activeLanguage = data.activeLanguage;
            firstTime = data.firstTime;
            musicVolume = data.musicVolume;
            sfxVolume = data.sfxVolume;
        }
    }

    /// <summary>
    /// Función que carga las variables de las opciones.
    /// </summary>
    public void SaveOptions()
    {
        OptionsData data = new OptionsData
        {
            activeLanguage = activeLanguage,
            firstTime = firstTime,
            musicVolume = musicVolume,
            sfxVolume = sfxVolume,
        };

        string json = JsonUtility.ToJson(data);

        string path = Application.persistentDataPath + "/Options.json";

        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(json);
        }
    }
}