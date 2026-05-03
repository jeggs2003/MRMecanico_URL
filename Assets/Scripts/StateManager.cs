using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Módulo de Lógica — Gestión de Estados
/// Controla qué información se muestra al tocar la pieza
/// Estados: Idle → Info → Precio → Clip → Idle
/// </summary>
public class StateManager : MonoBehaviour
{
    // ── Estados posibles ──────────────────────────────────────
    public enum EstadoPieza { Idle, Info, Precio, Clip }

    [Header("Estado Actual")]
    public EstadoPieza estadoActual = EstadoPieza.Idle;

    [Header("Paneles UI — asignar desde Inspector")]
    public GameObject panelInfo;       // Panel con especificaciones técnicas
    public GameObject panelPrecio;     // Panel con precio en Q.
    public GameObject botonCorte;      // Botón para activar clipping
    public GameObject botonDesensamblar;

    [Header("Referencias")]
    public ClippingPlaneController clippingController;
    public RepuestoAPIService apiService;

    void Start()
    {
        // Auto-asigna Mesh Colliders a todos los hijos
        MeshFilter[] meshes = GetComponentsInChildren<MeshFilter>();
        foreach (MeshFilter mf in meshes)
        {
            MeshCollider col = mf.gameObject.GetComponent<MeshCollider>();
            if (col == null)
                col = mf.gameObject.AddComponent<MeshCollider>();
            col.sharedMesh = mf.sharedMesh;
        }

        AplicarEstado(EstadoPieza.Idle);
    }

    /// <summary>
    /// Se llama al tocar la pieza en pantalla
    /// </summary>
    public void OnPiezaTocada()
    {
        // Cicla entre estados: Idle → Info → Precio → Idle
        switch (estadoActual)
        {
            case EstadoPieza.Idle:
                AplicarEstado(EstadoPieza.Info);
                break;

            case EstadoPieza.Info:
                AplicarEstado(EstadoPieza.Precio);
                // Solicita el precio a la API simulada
                if (apiService != null)
                    apiService.ObtenerPrecio(gameObject.name);
                break;

            case EstadoPieza.Precio:
                AplicarEstado(EstadoPieza.Idle);
                break;
        }
    }

    /// <summary>
    /// Activa/desactiva el corte transversal desde botón UI
    /// </summary>
    public void OnBotonCorte()
    {
        if (clippingController != null)
            clippingController.ToggleCorte();

        AplicarEstado(EstadoPieza.Clip);
    }

    /// <summary>
    /// Aplica visualmente el estado correspondiente
    /// </summary>
    private void AplicarEstado(EstadoPieza nuevoEstado)
    {
        estadoActual = nuevoEstado;

        // Oculta todo primero
        if (panelInfo != null) panelInfo.SetActive(false);
        if (panelPrecio != null) panelPrecio.SetActive(false);
        if (botonCorte != null) botonCorte.SetActive(false);
        if (botonDesensamblar != null) botonDesensamblar.SetActive(false); // en el ocultar todo
        

        // Muestra solo lo que corresponde al estado
        switch (estadoActual)
        {
            case EstadoPieza.Idle:
                // Todo oculto, esperando toque
                break;

            case EstadoPieza.Info:
                if (panelInfo != null) panelInfo.SetActive(true);
                if (botonCorte != null) botonCorte.SetActive(true);
                if (botonDesensamblar != null) botonDesensamblar.SetActive(true); 
                break;

            case EstadoPieza.Precio:
                if (panelInfo != null) panelInfo.SetActive(true);
                if (panelPrecio != null) panelPrecio.SetActive(true);
                if (botonCorte != null) botonCorte.SetActive(true);
                if (botonDesensamblar != null) botonDesensamblar.SetActive(true);  
                break;

            case EstadoPieza.Clip:
                if (panelInfo != null) panelInfo.SetActive(true);
                if (botonCorte != null) botonCorte.SetActive(true);

                break;
        }
    }

    /// <summary>
    /// Detecta toque sobre la pieza mediante Raycast
    /// </summary>
    void Update()
    {
        // Compatible con New Input System y Old
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
                DetectarToque(touch.position);
        }

        // Para probar en el editor con el mouse
        if (Input.GetMouseButtonDown(0))
            DetectarToque(Input.mousePosition);
    }

    private void DetectarToque(Vector2 posicion)
    {
        Ray ray = Camera.main.ScreenPointToRay(posicion);
        RaycastHit hit;

        Debug.Log($"[Touch] Toque en posición: {posicion}");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.Log($"[Raycast] Impactó: {hit.transform.name}");

            if (hit.transform.IsChildOf(transform) || hit.transform == transform)
            {
                Debug.Log("[StateManager] ¡Pieza tocada! Cambiando estado.");
                OnPiezaTocada();
            }
            else
            {
                Debug.Log($"[Raycast] Objeto no es hijo: {hit.transform.name}");
            }
        }
        else
        {
            Debug.Log("[Raycast] No impactó nada");
        }
    }

 
}
