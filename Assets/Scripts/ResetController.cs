using UnityEngine;

public class ResetController : MonoBehaviour
{
    private Vector3 _posicionOriginal;
    private Quaternion _rotacionOriginal;
    private Vector3 _escalaOriginal;

    private ExplodedViewController _explodedView;
    private AutoRotateController _autoRotate;

    void Start()
    {
        _posicionOriginal = transform.localPosition;
        _rotacionOriginal = transform.localRotation;
        _escalaOriginal = transform.localScale;

        _explodedView = GetComponent<ExplodedViewController>();
        _autoRotate = GetComponent<AutoRotateController>();
    }

    public void ResetearPieza()
    {
        // Resetea transform
        transform.localPosition = _posicionOriginal;
        transform.localRotation = _rotacionOriginal;
        transform.localScale = _escalaOriginal;

        // Resetea desemblaje si está activo
        if (_explodedView != null && _explodedView.explosionActiva)
            _explodedView.ToggleExplosion();

        // Detiene rotación automática si está activa
        if (_autoRotate != null && _autoRotate.rotacionActiva)
            _autoRotate.ToggleRotacion();

        Debug.Log("[Reset] Pieza restaurada completamente");
    }
}