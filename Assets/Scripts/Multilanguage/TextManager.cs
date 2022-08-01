using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase presente en cada texto del juego y que se encarga de modificarlo de acuerdo con el idioma activo.
/// </summary>
public class TextManager : MonoBehaviour
{
    /// <summary>
    /// El asset que contiene los textos necesarios.
    /// </summary>
    [SerializeField] MultilanguageText multilanguageText = null;

    private void OnEnable()
    {
        ChangeLanguage(MultilanguageManager.multilanguageManager.activeLanguage);
    }

    private void Start()
    {
        ChangeLanguage(MultilanguageManager.multilanguageManager.activeLanguage);
    }

    /// <summary>
    /// Función que modifica el texto de acuerdo con el idioma activo.
    /// </summary>
    /// <param name="newLanguage">Eñ código del idioma que queremos activar ("EN" o "ES").</param>
    void ChangeLanguage(string newLanguage)
    {
        Text text = GetComponent<Text>();

        switch (newLanguage)
        {
            case "EN":
                text.text = multilanguageText.english;
                break;
            case "ES":
                text.text = multilanguageText.spanish;
                break;
        }
    }
}