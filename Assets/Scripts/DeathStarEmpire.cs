using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase usada por la Estrella de la Muerte en el modo Imperio.
/// </summary>
public class DeathStarEmpire : MonoBehaviour
{
    /// <summary>
    /// El panel de Game Over que aparece al destruirse la Estrella de la Muerte.
    /// </summary>
    [SerializeField] GameObject panelGameOver = null;
    /// <summary>
    /// El cronómetro en la parte superior de la pantalla.
    /// </summary>
    [SerializeField] Timer timer = null;
    /// <summary>
    /// La salud máxima de la Estrella de la Muerte.
    /// </summary>
    [SerializeField] int maxHealth = 3000;
    /// <summary>
    /// La salud actual de la Estrella de la Muerte.
    /// </summary>
    int health;
    /// <summary>
    /// El slider de la barra de salud.
    /// </summary>
    [SerializeField] Slider sliderHealth = null;
    /// <summary>
    /// El panel con el texto que mostrará la salud restante.
    /// </summary>
    [SerializeField] Text textHealth = null;
    /// <summary>
    /// Las partículas que aparecerán al explotar la Estrella de la Muerte.
    /// </summary>
    [SerializeField] GameObject deadParticles = null;
    /// <summary>
    /// Los textos traducidos que aparecerán en la barra de salud.
    /// </summary>
    [SerializeField] MultilanguageText healthStringObject = null;
    /// <summary>
    /// El texto traducido que aparece en la barra de salud.
    /// </summary>
    string healthString;

    private void Start()
    {
        switch (MultilanguageManager.multilanguageManager.activeLanguage)
        {
            case "EN":
                healthString = healthStringObject.english;
                break;
            case "ES":
                healthString = healthStringObject.spanish;
                break;
        }

        health = maxHealth;
        sliderHealth.maxValue = maxHealth;
        sliderHealth.value = maxHealth;
        textHealth.text = healthString + "100 %";
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("BulletEnemy") || collision.gameObject.CompareTag("BulletPlayer"))
        {
            collision.gameObject.SetActive(false);

            health--;
            sliderHealth.value = health;
            textHealth.text = (healthString + (health * 100 / maxHealth) + " %");

            if (health <= 0)
            {
                textHealth.text = (healthString + "0 %");
                Instantiate(deadParticles, transform.position, transform.rotation);
                panelGameOver.SetActive(true);
                timer.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Destroy(gameObject);
            }
        }
    }
}