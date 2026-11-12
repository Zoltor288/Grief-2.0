using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class InactivityVideoPlayer : MonoBehaviour
{
    [Header("Configuración")]
    public float inactivityThreshold = 5f; // Tiempo sin actividad para mostrar el video
    public VideoClip idleVideoClip;        // Video a reproducir

    [Header("Referencias")]
    public RawImage videoDisplay;          // UI para mostrar el video
    public AudioSource audioSource;        // Fuente de audio opcional

    private VideoPlayer videoPlayer;
    private float inactivityTimer;
    private bool videoPlaying;

    void Start()
    {
        // Configurar VideoPlayer
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.clip = idleVideoClip;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = new RenderTexture((int)videoDisplay.rectTransform.rect.width,
                                                     (int)videoDisplay.rectTransform.rect.height, 24);
        videoDisplay.texture = videoPlayer.targetTexture;

        // Configurar audio si existe
        if (audioSource != null)
        {
            videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
            videoPlayer.SetTargetAudioSource(0, audioSource);
        }

        // Inicializar estado
        videoDisplay.gameObject.SetActive(false);
        videoPlaying = false;
        inactivityTimer = 0f;
    }

    void Update()
    {
        // Detectar actividad del usuario
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            ResetInactivityTimer();

            // Ocultar video si está visible
            if (videoPlaying)
            {
                StopVideo();
            }
        }
        else
        {
            inactivityTimer += Time.deltaTime;

            // Mostrar video después de inactividad
            if (!videoPlaying && inactivityTimer >= inactivityThreshold)
            {
                PlayVideo();
            }
        }
    }

    public void OnVideoClicked()
    {
        StopVideo();
        ResetInactivityTimer();
    }

    private void PlayVideo()
    {
        videoDisplay.gameObject.SetActive(true);
        videoPlayer.Play();
        videoPlaying = true;
    }

    private void StopVideo()
    {
        videoPlayer.Stop();
        videoDisplay.gameObject.SetActive(false);
        videoPlaying = false;
    }

    private void ResetInactivityTimer()
    {
        inactivityTimer = 0f;
    }
}
