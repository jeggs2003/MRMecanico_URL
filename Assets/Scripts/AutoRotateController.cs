using UnityEngine;
using TMPro;

public class AutoRotateController : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidadRotacion = 30f;
    public bool rotacionActiva = false;

    public void ToggleRotacion()
    {
        rotacionActiva = !rotacionActiva;
        Debug.Log("[AutoRotate] Rotación: " + rotacionActiva);
    }

    void Update()
    {
        if (!rotacionActiva) return;
        transform.Rotate(Vector3.up, velocidadRotacion * Time.deltaTime, Space.World);
    }
}