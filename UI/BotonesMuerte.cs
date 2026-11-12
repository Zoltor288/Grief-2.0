using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonesMuerte : MonoBehaviour
{
    [SerializeField] private GameObject CanvasMuerte;
    public void RestartCurrentScene()
    {
        CanvasMuerte.SetActive(false);
        // Obtener la escena actual y recargarla
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
