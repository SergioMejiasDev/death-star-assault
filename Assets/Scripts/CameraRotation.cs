using UnityEngine;

/// <summary>
/// Clase encargada de la rotación de la cámara en el menú principal.
/// </summary>
public class CameraRotation : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.up*-2.0f*Time.deltaTime);
    }
}