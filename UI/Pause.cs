using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    Canvas pause;
    bool active;
    [SerializeField] private GameObject Ajustes;
    [SerializeField] private GameObject Pausa;
    [SerializeField] private GameObject Video;

    private void Awake()
    {
        pause = GetComponent<Canvas>();
        pause.enabled = false;
        Video.SetActive(false); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseState();
        }
    }

    public void Restart() 
    {
        TogglePauseState();
    }

    private void TogglePauseState()
    {
        active = !active;

        pause.enabled = active;
        Video.SetActive(active); 
        Time.timeScale = active ? 0 : 1;
    }

    public void Controles()
    {
        Pausa.SetActive(false);
        Ajustes.SetActive(true);
    }

    public void DesactivarControles()
    {
        Pausa.SetActive(true);
        Ajustes.SetActive(false);
    }
}
