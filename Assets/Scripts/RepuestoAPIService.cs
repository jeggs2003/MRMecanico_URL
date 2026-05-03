using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Módulo de Lógica — Simulación de API de Precios
/// Simula la obtención de precios de repuestos en Quetzales
/// En producción real, aquí iría una llamada HTTP a un backend
/// </summary>
public class RepuestoAPIService : MonoBehaviour
{
    [Header("UI — Panel de Precio")]
    public TMP_Text textoPrecio;
    public TMP_Text textoNombrePieza;
    public TMP_Text textoEstado;

    [Header("Configuración")]
    public float tiempoSimulacion = 1.2f;  // Simula delay de red

    // ── Base de datos simulada de precios ─────────────────────
    private Dictionary<string, RepuestoData> _catalogoPrecios;

    // Estructura de datos de un repuesto
    [System.Serializable]
    public class RepuestoData
    {
        public string nombre;
        public float precio;
        public string estado;      // "Disponible", "Bajo Stock", "Agotado"
        public string proveedor;
    }

    void Awake()
    {
        _catalogoPrecios = new Dictionary<string, RepuestoData>()
        {
            {
                "tinker", new RepuestoData {
                    nombre    = "Disco de Freno Brembo",
                    precio    = 450.00f,
                    estado    = "Disponible",
                    proveedor = "Repuestos García, Zona 12"
                }
            },
            {
                "PistonDiagram", new RepuestoData {
                    nombre    = "Pistón de Motor STD",
                    precio    = 875.00f,
                    estado    = "Bajo Stock",
                    proveedor = "AutoPartes del Sur, Zona 4"
                }
            },
            {
                "default", new RepuestoData {
                    nombre    = "Repuesto General",
                    precio    = 299.00f,
                    estado    = "Disponible",
                    proveedor = "Consultar con proveedor"
                }
            }
        };
    }

    /// <summary>
    /// Simula una llamada a API y muestra el precio en el Canvas AR
    /// </summary>
    public void ObtenerPrecio(string nombrePieza)
    {
        StartCoroutine(SimularLlamadaAPI(nombrePieza));
    }

    private System.Collections.IEnumerator SimularLlamadaAPI(string nombrePieza)
    {
        // Muestra estado de carga mientras "espera la red"
        if (textoPrecio != null)
            textoPrecio.text = "Consultando precio...";

        // Simula el delay de una llamada HTTP real
        yield return new WaitForSeconds(tiempoSimulacion);

        // Busca en el catálogo, usa "default" si no encuentra
        string key = _catalogoPrecios.ContainsKey(nombrePieza)
                     ? nombrePieza
                     : "default";

        RepuestoData data = _catalogoPrecios[key];

        // Actualiza la UI con los datos obtenidos
        if (textoPrecio != null)
            textoPrecio.text = $"Precio estimado: Q{data.precio:F2}";

        if (textoNombrePieza != null)
            textoNombrePieza.text = data.nombre;

        if (textoEstado != null)
        {
            textoEstado.text = data.estado;

            // Color según disponibilidad
            textoEstado.color = data.estado switch
            {
                "Disponible" => Color.green,
                "Bajo Stock" => Color.yellow,
                "Agotado" => Color.red,
                _ => Color.white
            };
        }

        Debug.Log($"[API] {data.nombre} — Q{data.precio:F2} — {data.proveedor}");
    }
}