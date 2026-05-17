using UnityEngine;

public class TouchRotationController : MonoBehaviour
{
    [Header("Configuración")]
    public float rotationSpeed = 0.3f;
    public float zoomSpeed = 0.005f;
    public float minScale = 0.01f;
    public float maxScale = 0.3f;

    private Vector2 _lastTouchPos;
    private bool _isDragging = false;
    private bool _sobreModelo = false;

    void Update()
    {
        if (Input.touchCount == 0) return;

        // --- UN DEDO: Rotar solo si el toque empezó sobre el modelo ---
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // Verifica si el toque es sobre este modelo
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    _sobreModelo = hit.transform.IsChildOf(transform)
                                   || hit.transform == transform;
                }
                else
                {
                    _sobreModelo = false;
                }

                _lastTouchPos = touch.position;
                _isDragging = true;
            }
            else if (touch.phase == TouchPhase.Moved && _isDragging && _sobreModelo)
            {
                Vector2 delta = touch.position - _lastTouchPos;

                float rotY = -delta.x * rotationSpeed;
                float rotX = delta.y * rotationSpeed;

                // Usa quaternions para evitar Gimbal Lock
                Quaternion rotacionY = Quaternion.AngleAxis(rotY, Vector3.up);
                Quaternion rotacionX = Quaternion.AngleAxis(rotX, Camera.main.transform.right);

                transform.rotation = rotacionY * rotacionX * transform.rotation;

                _lastTouchPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                _isDragging = false;
                _sobreModelo = false;
            }
        }

        
        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            float prevDist = Vector2.Distance(
                t0.position - t0.deltaPosition,
                t1.position - t1.deltaPosition
            );
            float currDist = Vector2.Distance(t0.position, t1.position);
            float diff = currDist - prevDist;

            // Usa los mismos límites que ZoomController
            float escalaActual = transform.localScale.x;
            float nuevaEscala = Mathf.Clamp(
                escalaActual + diff * 0.00005f,  // muy gradual
                0.005f,   // escalaMinima
                0.03f     // escalaMaxima
            );
            transform.localScale = Vector3.one * nuevaEscala;
        }
    }
}