using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplodedViewController : MonoBehaviour
{
    [Header("Configuración")]
    public float distanciaExplosion = 0.15f;
    public float duracionAnimacion = 1.2f;

    [Header("Estado")]
    public bool explosionActiva = false;

    // Guarda la posición original de cada pieza
    private Dictionary<Transform, Vector3> _posicionesOriginales
        = new Dictionary<Transform, Vector3>();

    void Start()
    {
        // Guarda posiciones originales de todos los hijos
        foreach (Transform hijo in GetComponentsInChildren<Transform>())
        {
            if (hijo != transform)
                _posicionesOriginales[hijo] = hijo.localPosition;
        }
    }

    public void ToggleExplosion()
    {
        explosionActiva = !explosionActiva;

        if (explosionActiva)
            StartCoroutine(Explotar());
        else
            StartCoroutine(Ensamblar());
    }

    private IEnumerator Explotar()
    {
        Debug.Log("[Explosion] Iniciando con " + _posicionesOriginales.Count + " piezas");
        float tiempo = 0f;
        Vector3 centro = transform.position;

        Dictionary<Transform, Vector3> inicios = new Dictionary<Transform, Vector3>();
        Dictionary<Transform, Vector3> destinos = new Dictionary<Transform, Vector3>();

        foreach (Transform hijo in _posicionesOriginales.Keys)
        {
            if (hijo == null) continue;
            inicios[hijo] = hijo.localPosition;
            Vector3 direccion = (hijo.position - centro).normalized;
            if (direccion == Vector3.zero)
                direccion = Random.onUnitSphere;
            destinos[hijo] = _posicionesOriginales[hijo] +
                 (transform.InverseTransformDirection(direccion) * distanciaExplosion);
            Debug.Log($"[Explosion] Pieza: {hijo.name} → dirección: {direccion}");
        }

        while (tiempo < duracionAnimacion)
        {
            tiempo += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, tiempo / duracionAnimacion);
            foreach (Transform hijo in inicios.Keys)
            {
                if (hijo == null) continue;
                hijo.localPosition = Vector3.Lerp(inicios[hijo], destinos[hijo], t);
            }
            yield return null;
        }
    }

    private IEnumerator Ensamblar()
    {
        float tiempo = 0f;

        Dictionary<Transform, Vector3> inicios
            = new Dictionary<Transform, Vector3>();

        foreach (Transform hijo in _posicionesOriginales.Keys)
        {
            if (hijo != null)
                inicios[hijo] = hijo.localPosition;
        }

        // Anima cada pieza de vuelta a su posición original
        while (tiempo < duracionAnimacion)
        {
            tiempo += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, tiempo / duracionAnimacion);

            foreach (Transform hijo in inicios.Keys)
            {
                if (hijo == null) continue;
                hijo.localPosition = Vector3.Lerp(
                    inicios[hijo],
                    _posicionesOriginales[hijo],
                    t
                );
            }
            yield return null;
        }

        // Asegura posiciones exactas al final
        foreach (Transform hijo in _posicionesOriginales.Keys)
        {
            if (hijo != null)
                hijo.localPosition = _posicionesOriginales[hijo];
        }
    }
}