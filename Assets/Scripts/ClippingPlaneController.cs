using UnityEngine;
using System.Collections;

public class ClippingPlaneController : MonoBehaviour
{
    [Header("Configuración")]
    public float duracionCorte = 1.5f;
    public bool corteActivo = false;

    private Renderer[] _renderers;
    private Vector3 _escalaOriginal;

    void Start()
    {
        _renderers = GetComponentsInChildren<Renderer>();
        _escalaOriginal = transform.localScale;
    }

    public void ToggleCorte()
    {
        corteActivo = !corteActivo;

        if (corteActivo)
            StartCoroutine(AnimarCorte());
        else
            ResetearModelo();
    }

    private IEnumerator AnimarCorte()
    {
        float tiempo = 0f;
        Vector3 escalaInicio = transform.localScale;
        // Aplana el modelo en Y para simular el corte transversal
        Vector3 escalaFin = new Vector3(
            _escalaOriginal.x,
            _escalaOriginal.y * 0.01f,
            _escalaOriginal.z
        );

        while (tiempo < duracionCorte)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracionCorte;
            transform.localScale = Vector3.Lerp(escalaInicio, escalaFin, t);
            yield return null;
        }

        // Muestra solo la mitad ocultando renderers alternos
        for (int i = 0; i < _renderers.Length; i++)
        {
            if (i % 2 == 0)
                _renderers[i].enabled = false;
        }

        transform.localScale = _escalaOriginal;
    }

    private void ResetearModelo()
    {
        StopAllCoroutines();
        transform.localScale = _escalaOriginal;

        foreach (Renderer r in _renderers)
            r.enabled = true;
    }
}