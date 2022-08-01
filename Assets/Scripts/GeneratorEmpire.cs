using System.Collections;
using UnityEngine;

/// <summary>
/// Clase encargada de generar enemigos en el modo Imperio.
/// </summary>
public class GeneratorEmpire : MonoBehaviour
{
    /// <summary>
    /// Tiempo que pasa desde que se genera un enemigo hasta que aparece otro nuevo.
    /// </summary>
    [SerializeField] float timeBetweenEnemies = 4;
    /// <summary>
    /// La nave del jugador.
    /// </summary>
    [SerializeField] GameObject player = null;

    private void Start()
    {
        StartCoroutine(Spawner());
    }

    /// <summary>
    /// Función encargada de generar nuevos enemigos.
    /// </summary>
    public void GenerateEnemy()
    {
        Vector3 offset = new Vector3(Random.Range(-2000, 2000), 0, 0);
        GameObject enemy = ObjectPooler.SharedInstance.GetPooledObject("Enemy");

        if (enemy != null)
        {
            enemy.SetActive(true);
            enemy.GetComponent<EnemyEmpire>().health = enemy.GetComponent<EnemyEmpire>().maxHealth;
            enemy.GetComponent<EnemyEmpire>().dead = false;
            enemy.GetComponent<EnemyEmpire>().sliderHealth.value = enemy.GetComponent<EnemyEmpire>().health;

            enemy.transform.position = transform.position + offset;
            enemy.transform.rotation = transform.rotation;
        }

        GameManager.manager.enemyNumber++;
    }

    /// <summary>
    /// Corrutina que genera un nuevo enemigo tras unos segundos.
    /// </summary>
    /// <returns></returns>
    IEnumerator Spawner()
    {
        while (true)
        {
            if (GameManager.manager.enemyNumber <= 30)
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