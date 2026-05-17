using UnityEngine;

public class ZoomController : MonoBehaviour
{
    [Header("Configuración")]
    public float incrementoZoom = 0.0005f;
    public float escalaMinima = 0.005f;
    public float escalaMaxima = 0.03f;

    public void ZoomIn()
    {
        float escalaActual = transform.localScale.x;
        float nuevaEscala = Mathf.Clamp(
            escalaActual + incrementoZoom,
            escalaMinima,
            escalaMaxima
        );
        transform.localScale = Vector3.one * nuevaEscala;
        Debug.Log("[Zoom] In → " + nuevaEscala);
    }

    public void ZoomOut()
    {
        float escalaActual = transform.localScale.x;
        float nuevaEscala = Mathf.Clamp(
            escalaActual - incrementoZoom,
            escalaMinima,
            escalaMaxima
        );
        transform.localScale = Vector3.one * nuevaEscala;
        Debug.Log("[Zoom] Out → " + nuevaEscala);
    }
}