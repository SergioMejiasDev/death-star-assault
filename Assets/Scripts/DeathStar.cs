using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase usada por la Estrella de la Muerte en el modo Alianza.
/// </summary>
public class DeathStar : MonoBehaviour
{
    /// <summary>
    /// Posición donde se generarán las naves enemigas.
    /// </summary>
    [SerializeField] Transform generationPoint = null;
    /// <summary>
    /// Máximo de enemigos que se generarán.
    /// </summary>
    [SerializeField] int maxEnemies = 100;
    /// <summary>
    /// Tiempo en segundos que pasarán desde que se genera un enemigo hasta que aparece el siguiente.
    /// </summary>
    [SerializeField] float timeBetweenEnemies = 2;
    /// <summary>
    /// El panel que se activa al destruir la Estrella de la Muerte.
    /// </summary>
    [SerializeField] GameObject panelVictory = null;
    /// <summary>
    /// El cronómetro que aparece en la parte superior de la pantalla.
    /// </summary>
    [SerializeField] Timer timer = null;
    /// <summary>
    /// La nave del jugador.
    /// </summary>
    [SerializeField] GameObject player = null;

    /// <summary>
    /// La salud máxima de la Estrella de la Muerte.
    /// </summary>
    [SerializeField] int maxHealth = 200;
    /// <summary>
    /// La salud actual de la Estrella de la Muerte.
    /// </summary>
    int health;
    /// <summary>
    /// El slider de la barra de salud.
    /// </summary>
    [SerializeField] Slider sliderHealth = null;
    /// <summary>
    /// El panel con el texto que indica la salud restante.
    /// </summary>
    [SerializeField] Text textHealth = null;
    /// <summary>
    /// Las partículas que aparecerán cuando explote la Estrella de la Muerte.
    /// </summary>
    [SerializeField] GameObject deadParticles = null;
    /// <summary>
    /// El asset con los textos traducidos de la barra de salud.
    /// </summary>
    [SerializeField] MultilanguageText healthStringObject = null;
    /// <summary>
    /// El texto traducido que se va a mostrar en la barra de salud.
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

        StartCoroutine(Spawner());
        health = maxHealth;
        sliderHealth.maxValue = maxHealth;
        sliderHealth.value = maxHealth;
        textHealth.text = (healthString + "100 %");
    }

    /// <summary>
    /// Función que genera un nuevo enemigo.
    /// </summary>
    public void GenerateEnemy()
    {
        Vector3 offset = new Vector3(Random.Range(-25, 25), Random.Range(-25, 25), 0);
        GameObject enemy = ObjectPooler.SharedInstance.GetPooledObject("Enemy");

        if (enemy != null)
        {
            enemy.SetActive(true);
            enemy.GetComponent<Enemy>().health = enemy.GetComponent<Enemy>().maxHealth;
            enemy.GetComponent<Enemy>().dead = false;
            enemy.GetComponent<Enemy>().sliderHealth.value = enemy.GetComponent<Enemy>().health;

            enemy.transform.position = generationPoint.position + offset;
            enemy.transform.rotation = generationPoint.rotation;
        }

        GameManager.manager.enemyNumber++;
        maxEnemies--;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BulletPlayer") && (GameManager.manager.enemyNumber <= 0 && maxEnemies <= 0))
        {
            health--;
            sliderHealth.value = health;
            textHealth.text = (healthString + (health * 100 / maxHealth) + " %");

            other.gameObject.SetActive(false);
            
            if (health <= 0)
            {
                textHealth.text = (healthString + "0 %");
                Instantiate(deadParticles, transform.position, transform.rotation);
                panelVictory.SetActive(true);
                timer.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Corrutina encargada de generar nuevos enemigos pasados unos segundos.
    /// </summary>
    /// <returns></returns>
    IEnumerator Spawner()
    {
        while (true)
        {
            if (maxEnemies > 0 && GameManager.manager.enemyNumber <= 30)
            {
                if (player != null)
                {
                    GenerateEnemy();
                }
            }

            yield return new WaitForSeconds(timeBetweenEnemies);
        }
    }
}