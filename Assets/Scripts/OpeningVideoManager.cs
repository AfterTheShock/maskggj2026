using UnityEngine;
using UnityEngine.Video; // Necesario para manejar videos

public class OpeningVideoManager : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private VideoPlayer OpeningPlayer;
    [SerializeField] private GameObject MainMenuCanvas;
    [SerializeField] private GameObject OpeningVideoCanvas;

    void Start()
    {
        // Nos aseguramos de que el video comience desde el principio
        if (OpeningPlayer != null)
        {
            // Suscribimos una función al evento de "fin de video"
            OpeningPlayer.loopPointReached += OnVideoFinished;
        }
        else
        {
            Debug.LogError("No se encontró el OpeningPlayer en el script.");
        }
    }

    // Esta función se ejecuta automáticamente cuando el video termina
    void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("Video terminado. Cambiando al menú principal...");

        // Activamos el Menú
        if (MainMenuCanvas != null) MainMenuCanvas.SetActive(true);

        // Apagamos el Video
        if (OpeningVideoCanvas != null) OpeningVideoCanvas.SetActive(false);
    }
}