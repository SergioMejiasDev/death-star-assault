using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase que controla las funciones principales del jugador en el modo Imperio.
/// </summary>
public class PlayerEmpire : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// Velocidad de movimiento del jugador.
    /// </summary>
    [Header("Movement")]
    float speed;
    /// <summary>
    /// Velocidad de rotación de la nave.
    /// </summary>
    float speedRot = 1;

    /// <summary>
    /// Posición donde se generarán los disparos.
    /// </summary>
    [Header("Shoot")]
    [SerializeField] Transform[] shootPoint = null;
    /// <summary>
    /// El número del cañón al que le toca disparar.
    /// </summary>
    int cannonNumber = 0;
    /// <summary>
    /// Verdadero si disparan todos los cañones a la vez. Falso si disparan de uno en uno.
    /// </summary>
    bool shootAll;
    /// <summary>
    /// Momento en el que se ha realizado el disparo.
    /// </summary>
    float timeLastShoot;
    /// <summary>
    /// Tiempo entre disparo y disparo.
    /// </summary>
    float cadency = 0.25f;

    /// <summary>
    /// Panel con el menú de pausa.
    /// </summary>
    [Header("Panels")]
    [SerializeField] GameObject panelPause = null;
    /// <summary>
    /// Panel con el menú de Game Over.
    /// </summary>
    [SerializeField] GameObject panelGameOver = null;
    /// <summary>
    /// La clase que controla el cronómetro.
    /// </summary>
    [SerializeField] Timer timer = null;

    /// <summary>
    /// La salud máxima de la nave.
    /// </summary>
    [Header("Health")]
    readonly int maxHealth = 100;
    /// <summary>
    /// La salud actual de la nave.
    /// </summary>
    int health;
    /// <summary>
    /// Slider con la barra de salud de la nave.
    /// </summary>
    [SerializeField] Slider sliderHealth = null;
    /// <summary>
    /// Panel que indica la salud restante.
    /// </summary>
    [SerializeField] Text textHealth = null;
    /// <summary>
    /// Asset con los textos traducidos que pueden aparecer en el panel de la salud.
    /// </summary>
    [SerializeField] MultilanguageText healthStringObject = null;
    /// <summary>
    /// El texto que aparece en el panel de la salud.
    /// </summary>
    string healthString;

    #endregion

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

    private void Update()
    {
        if (Time.timeScale != 0)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime * Input.GetAxis("Vertical"));
            transform.Translate(Vector3.right * speed * Time.deltaTime * Input.GetAxis("Horizontal"));
            transform.Translate(Vector3.up * speed * Time.deltaTime * Input.GetAxis("UpDown"));

            transform.Rotate(Vector3.up * speedRot * Input.GetAxis("Mouse X"));
            transform.Rotate(Vector3.left * speedRot * Input.GetAxis("Mouse Y"));

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = 250.0f;
            }

            else
            {
                speed = 150.0f;
            }

            if (Input.GetButton("Fire1") && Time.time - timeLastShoot > cadency)
            {
                Shoot();
            }

            if (Input.GetButtonDown("Fire2"))
            {
                shootAll = !shootAll;
            }
        }
       
        if (Input.GetButtonDown("Cancel"))
        {
            panelPause.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            Time.timeScale = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BulletEnemy"))
        {
            other.gameObject.SetActive(false);

            health--;
            sliderHealth.value = health;
            textHealth.text = (healthString + (health * 100 / maxHealth) + " %");

            if (health <= 0)
            {
                Death();
            }
        }
        
        if (other.gameObject.CompareTag("DeathStar"))
        {
            Death();
        }
    }

    /// <summary>
    /// Función que se activa cuando la nave dispara.
    /// </summary>
    void Shoot()
    {
        timeLastShoot = Time.time;
        if (shootAll)
        {
            for (int i = 0; i < shootPoint.Length; i++)
            {
                GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("BulletPlayer");

                if (bullet != null)
                {
                    bullet.transform.position = shootPoint[i].position;
                    bullet.transform.rotation = shootPoint[i].rotation;
                    bullet.SetActive(true);
                }
            }
        }

        else
        {
            GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("BulletPlayer");

            if (bullet != null)
            {
                bullet.transform.position = shootPoint[cannonNumber].position;
                bullet.transform.rotation = shootPoint[cannonNumber].rotation;
                bullet.SetActive(true);
            }

            cannonNumber++;

            if (cannonNumber > 1)
            {
                cannonNumber = 0;
            }
        }
    }

    /// <summary>
    /// Función que reanuda la partida durante la pausa.
    /// </summary>
    public void ResumeGame()
    {
        panelPause.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }

    /// <summary>
    /// Función que se activa cuando el jugador muere.
    /// </summary>
    public void Death()
    {
        panelGameOver.SetActive(true);
        timer.enabled = false;
        health = 0;
        sliderHealth.value = 0;
        textHealth.text = (healthString + "0 %");
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Camera.main.transform.SetParent(null);
        
        GameObject deadParticles = ObjectPooler.SharedInstance.GetPooledObject("Particles");

        if (deadParticles != null)
        {
            deadParticles.SetActive(true);
            deadParticles.transform.position = transform.position;
            deadParticles.transform.rotation = transform.rotation;
        }
        
        Destroy(gameObject);
    }
}