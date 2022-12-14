using UnityEngine;

/// <summary>
/// Clase encargada de modificar los textos de acuerdo con el idioma activo.
/// </summary>
public class MultilanguageManager : MonoBehaviour
{
    public static MultilanguageManager multilanguageManager;

    /// <summary>
    /// El idioma activo ("EN" o "ES").
    /// </summary>
    public string activeLanguage;

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("LanguageManager");

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        else
        {
            multilanguageManager = this;

            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        activeLanguage = SaveManager.saveManager.activeLanguage;
    }

    /// <summary>
    /// Función encargada de cambiar el idioma de todos los textos del juego.
    /// </summary>
    /// <param name="newLanguage">El idioma que queremos activar.</param>
    public void ChangeLanguage(string newLanguage)
    {
        activeLanguage = newLanguage;

        SaveManager.saveManager.activeLanguage = newLanguage;
        SaveManager.saveManager.firstTime = true;
        SaveManager.saveManager.SaveOptions();
    }
}