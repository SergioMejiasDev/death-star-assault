using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase que se encarga de las funciones principales de las naves de la Alianza.
/// </summary>
public class EnemyEmpire : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// Dirección a la que se va a mover la nave.
    /// </summary>
    [Header("Movement")]
    Vector3 destiny;
    /// <summary>
    /// Velocidad de movimiento de la nave.
    /// </summary>
    readonly float speed = 150.0f;
    /// <summary>
    /// La posición del jugador.
    /// </summary>
    Transform player;
    /// <summary>
    /// La posición de la Estrella de la Muerte.
    /// </summary>
    Transform deathStar;

    /// <summary>
    /// Las posiciones donde se generarán los disparos.
    /// </summary>
    [Header("Shoot")]
    [SerializeField] Transform shootPoint = null;
    /// <summary>
    /// Momento en el que se realizó el último disparo.
    /// </summary>
    float timeLastShoot;
    /// <summary>
    /// Tiempo entre disparo y disparo.
    /// </summary>
    readonly float cadency = 0.25f;

    /// <summary>
    /// Salud máxima de la nave.
    /// </summary>
    [Header("Health")]
    public readonly int maxHealth = 2;
    /// <summary>
    /// Salud actual de la nave.
    /// </summary>
    public int health;
    /// <summary>
    /// Slider con la salud restante de la nave.
    /// </summary>
    public Slider sliderHealth;
    /// <summary>
    /// Verdadero si la nave está destruida, falso si no lo está.
    /// </summary>
    public bool dead;

    #endregion

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        deathStar = GameObject.FindGameObjectWithTag("DeathStar").transform;
        sliderHealth.maxValue = maxHealth;
        sliderHealth.value = maxHealth;
        dead = false;
        ChooseDestiny();
    }

    private void Update()
    {
        if (Time.timeScale != 0)
        {
            if ((player != null) && (Vector3.Distance(transform.position, player.position) < 300))
            {
                transform.LookAt(player);

                shootPoint.LookAt(player);

                if (Time.time - timeLastShoot > cadency)
                {
                    Shoot();
                }
            }

            else if ((deathStar != null) && (Vector3.Distance(transform.position, deathStar.position) < 500))
            {
                transform.LookAt(deathStar);

                if (Time.time - timeLastShoot > cadency)
                {
                    Shoot();
                }
            }

            else
            {
                transform.LookAt(destiny);

                transform.Translate(Vector3.forward * speed * Time.deltaTime);

                if (Vector3.Distance(destiny, transform.position) < 2)
                {
                    ChooseDestiny();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("BulletPlayer"))
        {
            collision.gameObject.SetActive(false);

            health--;
            sliderHealth.value = health;

            if (health <= 0 && !dead)
            {
                dead = true;
                GameManager.manager.UpdateScore(true);
                GameManager.manager.enemyNumber--;
                GameObject deadParticles = ObjectPooler.SharedInstance.GetPooledObject("Particles");
                
                if (deadParticles != null)
                {
                    deadParticles.SetActive(true);
                    deadParticles.transform.position = transform.position;
                    deadParticles.transform.rotation = transform.rotation;
                }

                gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Función que se activa cada vez que el enemigo dispara.
    /// </summary>
    void Shoot()
    {
        timeLastShoot = Time.time;
        
        GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("BulletEnemy");

        if (bullet != null)
        {
            bullet.transform.position = shootPoint.position;
            bullet.transform.rotation = shootPoint.rotation;
            bullet.SetActive(true);
        }
    }

    /// <summary>
    /// Función encargada de asignar un nuevo destino a la nave.
    /// </summary>
    void ChooseDestiny()
    {
        if (deathStar != null)
        {
            destiny = new Vector3(Random.Range(deathStar.position.x - 500, deathStar.position.x + 500), Random.Range(deathStar.position.y - 500, deathStar.position.y + 500), Random.Range(deathStar.position.z - 500, deathStar.position.z + 500));
        }
    }
}