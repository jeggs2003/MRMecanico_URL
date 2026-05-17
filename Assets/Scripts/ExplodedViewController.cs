using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplodedViewController : MonoBehaviour
{
    [Header("Configuración")]
    public float distanciaExplosion = 8f;
    public float duracionAnimacion = 1.2f;

    [Header("Estado")]
    public bool explosionActiva = false;

    // Guarda posición Y dirección de explosión fijas desde el inicio
    private Dictionary<Transform, Vector3> _posicionesOriginales
        = new Dictionary<Transform, Vector3>();
    private Dictionary<Transform, Vector3> _direccionesExplosion
        = new Dictionary<Transform, Vector3>();

    void Start()
    {
        // Direcciones fijas predefinidas para explosión consistente
        Vector3[] direccionesFijas = new Vector3[]
        {
        new Vector3( 1,  1,  0).normalized,
        new Vector3(-1,  1,  0).normalized,
        new Vector3( 0,  1,  1).normalized,
        new Vector3( 0,  1, -1).normalized,
        new Vector3( 1,  0,  1).normalized,
        new Vector3(-1,  0,  1).normalized,
        new Vector3( 1,  0, -1).normalized,
        new Vector3(-1,  0, -1).normalized,
        new Vector3( 0,  2,  0).normalized,
        };

        int i = 0;
        foreach (Transform hijo in GetComponentsInChildren<Transform>())
        {
            if (hijo == transform) continue;
            _posicionesOriginales[hijo] = hijo.localPosition;
            _direccionesExplosion[hijo] = direccionesFijas[i % direccionesFijas.Length];
            i++;
        }
    }

    public void ToggleExplosion()
    {
        explosionActiva = !explosionActiva;
        Debug.Log("[Explosion] ToggleExplosion — activo: " + explosionActiva);

        StopAllCoroutines();

        if (explosionActiva)
            StartCoroutine(Explotar());
        else
            StartCoroutine(Ensamblar());
    }

    private IEnumerator Explotar()
    {
        float tiempo = 0f;

        // Guarda posición actual como inicio (puede estar ensamblando a medias)
        Dictionary<Transform, Vector3> inicios = new Dictionary<Transform, Vector3>();
        Dictionary<Transform, Vector3> destinos = new Dictionary<Transform, Vector3>();

        foreach (Transform hijo in _posicionesOriginales.Keys)
        {
            if (hijo == null) continue;

            inicios[hijo] = hijo.localPosition;

            // Destino siempre en la misma dirección fija calculada al inicio
            destinos[hijo] = _posicionesOriginales[hijo]
                           + _direccionesExplosion[hijo] * distanciaExplosion;
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

        Dictionary<Transform, Vector3> inicios = new Dictionary<Transform, Vector3>();

        foreach (Transform hijo in _posicionesOriginales.Keys)
        {
            if (hijo != null)
                inicios[hijo] = hijo.localPosition;
        }

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

        // Posiciones exactas al terminar
        foreach (Transform hijo in _posicionesOriginales.Keys)
            if (hijo != null)
                hijo.localPosition = _posicionesOriginales[hijo];
    }
}