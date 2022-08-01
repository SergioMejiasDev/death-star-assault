using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Clase que contiene las funciones principales del juego.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    /// <summary>
    /// Las naves restantes del modo Alianza.
    /// </summary>
    int score;
    /// <summary>
    /// Las naves restantes del modo Imperio.
    /// </summary>
    int scoreEmpire;
    /// <summary>
    /// Panel que muestra las naves restantes del modo Alianza.
    /// </summary>
    [SerializeField] Text scoreText = null;
    /// <summary>
    /// Panel que muestra las naves restantes del modo Imperio.
    /// </summary>
    [SerializeField] Text scoreEmpireText = null;
    /// <summary>
    /// El número de enemigos en pantalla.
    /// </summary>
    public int enemyNumber;
    /// <summary>
    /// Todos los posibles paneles de la escena.
    /// </summary>
    [SerializeField] GameObject[] panels = null;

    /// <summary>
    /// Panel de carga.
    /// </summary>
    [SerializeField] GameObject loadingPanel = null;
    /// <summary>
    /// Imagen negra de transición entre escenas.
    /// </summary>
    [SerializeField] Image fadeImage = null;
    /// <summary>
    /// Texto de carga.
    /// </summary>
    [SerializeField] Text loadingText = null;

    private void Awake()
    {
        manager = this;
        SetInitialScore();
        Time.timeScale = 1;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (!SaveManager.saveManager.firstTime)
            {
                ManagePanels(panels[3]);
            }
        }
    }

    /// <summary>
    /// Función que establece los valores iniciales de naves restantes.
    /// </summary>
    void SetInitialScore()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            score = 100;
            scoreText.text = score.ToString();
        }

        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            scoreEmpire = 0;
            scoreEmpireText.text = scoreEmpire.ToString();
        }
    }

    /// <summary>
    /// Función que se activa cuando se destruye un enemigo.
    /// </summary>
    /// <param name="empireMode">Verdadero si estamos en el modo Imperio, falso si estamos en el modo Alianza.</param>
    public void UpdateScore(bool empireMode)
    {
        if (!empireMode)
        {
            score -= 1;
            scoreText.text = score.ToString();
        }

        else
        {
            scoreEmpire += 1;
            scoreEmpireText.text = scoreEmpire.ToString();
        }
    }

    /// <summary>
    /// Función que carga una nueva escena.
    /// </summary>
    /// <param name="buildIndex">Número de la escena que se va a cargar.</param>
    public void LoadScene(int buildIndex)
    {
        StartCoroutine(Loading(buildIndex));
    }

    /// <summary>
    /// Función que cierra el juego.
    /// </summary>
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// Función que abre y cierra los paneles del menú.
    /// </summary>
    /// <param name="panel">El panel que se va a abrir.</param>
    public void ManagePanels(GameObject panel)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }

        panel.SetActive(true);
    }

    /// <summary>
    /// Función encargada de cambiar el idioma de los textos en el juego.
    /// </summary>
    /// <param name="language">El código del idioma que queremos activar.</param>
    public void ChangeLanguage(string language)
    {
        MultilanguageManager.multilanguageManager.ChangeLanguage(language);
    }

    /// <summary>
    /// Corrutina que se activa cuando cambiamos de escena.
    /// </summary>
    /// <param name="sceneNumber">La escena que se va a cargar.</param>
    /// <returns></returns>
    IEnumerator Loading(int sceneNumber)
    {
        Time.timeScale = 1;
        loadingPanel.SetActive(true);

        Color imageColor = fadeImage.color;
        float alphaValue;

        while (fadeImage.color.a < 1)
        {
            alphaValue = imageColor.a + (2 * Time.deltaTime);
            imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
            fadeImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
            yield return null;
        }

        loadingText.enabled = true;

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(sceneNumber);
    }
}