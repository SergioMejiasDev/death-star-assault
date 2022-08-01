using System.Collections;
using UnityEngine;

/// <summary>
/// Clase encargada de resetear los parámetros de las balas generadas por pooling.
/// </summary>
public class Bullet : MonoBehaviour
{
    /// <summary>
    /// Sonido del disparo.
    /// </summary>
    [SerializeField] AudioSource shootSound = null;
    /// <summary>
    /// Componente TrailRenderer (estela de luz) de la bala.
    /// </summary>
    [SerializeField] TrailRenderer trailRenderer = null;
    /// <summary>
    /// Componente Rigidbody de la bala.
    /// </summary>
    [SerializeField] Rigidbody rb = null;

    void OnEnable()
    {
        shootSound.Play();
        trailRenderer.Clear();
        StartCoroutine(DestroyBullet());
    }

    private void OnDisable()
    {
        rb.velocity = Vector3.zero;
        StopAllCoroutines();
    }

    /// <summary>
    /// Corrutina encargada de destruir las balas tras unos segundos.
    /// </summary>
    /// <returns></returns>
    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(2);

        gameObject.SetActive(false);
    }
}