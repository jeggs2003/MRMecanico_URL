using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioController : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip audioDefectuoso;

    [Header("UI")]
    public GameObject botonAudio;
    public TMP_Text textoBoton;

    private AudioSource _audioSource;
    private bool _reproduciendo = false;

    void Start()
    {
        // Crea AudioSource automáticamente
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.loop = false;
    }

    public void ToggleAudio()
    {
        if (_reproduciendo)
        {
            _audioSource.Stop();
            _reproduciendo = false;

            if (textoBoton != null)
                textoBoton.text = "🔊 Sonido Defecto";
        }
        else
        {
            if (audioDefectuoso != null)
            {
                _audioSource.clip = audioDefectuoso;
                _audioSource.Play();
                _reproduciendo = true;

                if (textoBoton != null)
                    textoBoton.text = "⏹ Detener";
            }
        }
    }

    // Detecta cuando el audio termina naturalmente
    void Update()
    {
        if (_reproduciendo && !_audioSource.isPlaying)
        {
            _reproduciendo = false;
            if (textoBoton != null)
                textoBoton.text = "🔊 Sonido Defecto";
        }
    }
}