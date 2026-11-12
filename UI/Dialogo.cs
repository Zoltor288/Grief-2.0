//TMPro es un archivo de unity para poner las fuentes y Tmp_Text es para que busque la referencia 
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Dialogo : MonoBehaviour
{
    [SerializeField, TextArea(1, 4)] private string[] dialogueLines;
    [SerializeField] private TMP_Text DialogoTexto;

    private float typingTime = 0.05f;

    private bool didDialogueStart;
    private int LineIndex;

    [SerializeField] private AudioClip _AUDText;

    void Update()
    {
         if (!didDialogueStart)
         {
            StartDialogue();
         }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (DialogoTexto.text == dialogueLines[LineIndex])
            {
                NextDialogueLine();
            }
            else 
            { 
                StopAllCoroutines();
                DialogoTexto.text = dialogueLines[LineIndex];
            }
        }
    }

    //Corrutina para comenzar el dialogo , progresarlo
    private void StartDialogue()
    {
        didDialogueStart = true;
        LineIndex = 0;
        //Time.timeScale = 0f;
        StartCoroutine(ShowLine());
    }
    private void NextDialogueLine()
    {
        LineIndex++;
        if(LineIndex < dialogueLines.Length)
        {
           StartCoroutine(ShowLine());

        }
        else
        {
            didDialogueStart=false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            //Time.timeScale = 1f;
        }
    }
    // codigo para que el texto parezca de undertale
    private IEnumerator ShowLine()
    {
        DialogoTexto.text = string.Empty;
        foreach (char ch in dialogueLines[LineIndex])
        {
            DialogoTexto.text += ch;
            ControladorSonido.Instance.EjecutarSonido(_AUDText);
            yield return new WaitForSecondsRealtime(typingTime);
        }
    }
}
