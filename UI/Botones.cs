using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Botones : MonoBehaviour
{
    private bool active;
    [SerializeField] private GameObject Ajustes;
    [SerializeField] private GameObject MInicio;

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float delayAfterFade = 0.2f;

    public void Mainmenu() => StartCoroutine(LoadSceneWithFade("Menu inicio"));
    public void Mainmenu2() => StartCoroutine(LoadSceneWithFade("Menu inicio"));
    public void ModoHistoria() => StartCoroutine(LoadSceneWithFade("DialogoNegacion"));
    public void NivelIra() => StartCoroutine(LoadSceneWithFade("DialogoIra 1"));
    public void NivelNegociacion() => StartCoroutine(LoadSceneWithFade("DialogoNegociacion 1"));
    public void NivelDepresion() => StartCoroutine(LoadSceneWithFade("DialogoDepresion 1"));
    public void NivelAceptacion () => StartCoroutine(LoadSceneWithFade("DialogoAceptacion 1"));
    public void SelectorNiveles() => StartCoroutine(LoadSceneWithFade("SelectorNiveles"));
    public void ModoInfinito() => StartCoroutine(LoadSceneWithFade("ModoInfinito"));
    public void Creditos() => StartCoroutine(LoadSceneWithFade("Creditos"));

    public void Exit()
    {
        Application.Quit();
        Debug.Log("Se cierra");
    }

    public void Controles()
    {
        if (MInicio != null) MInicio.SetActive(false);
        if (Ajustes != null) Ajustes.SetActive(true);
    }

    public void DesactivarControles()
    {
        if (MInicio != null) MInicio.SetActive(true);
        if (Ajustes != null) Ajustes.SetActive(false);
    }

    private IEnumerator LoadSceneWithFade(string sceneName)
    {
        // Bloquear interacción con botones durante la transición
        SetButtonsInteractable(false);

        if (FadeController.Instance == null)
        {
            Debug.LogWarning("FadeController no encontrado. Cargando escena sin transición.");
            SceneManager.LoadScene(sceneName);
            ResetTimeScale();
            yield break;
        }

        yield return StartCoroutine(FadeController.Instance.FadeToBlack(fadeDuration));
        yield return new WaitForSeconds(delayAfterFade);

        SceneManager.LoadScene(sceneName);
        ResetTimeScale();

        // Reactivar botones en la nueva escena
        SetButtonsInteractable(true);

        yield return StartCoroutine(FadeController.Instance.FadeFromBlack(fadeDuration));
    }

    private void ResetTimeScale()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
    }

    // Método para bloquear/desbloquear botones
    private void SetButtonsInteractable(bool state)
    {
        Button[] allButtons = FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Button btn in allButtons)
        {
            btn.interactable = state;
        }
    }
}
