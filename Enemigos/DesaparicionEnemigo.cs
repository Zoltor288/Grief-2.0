using UnityEngine;

public class DesaparicionEnemigo : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        // Obtener referencias a los componentes al inicio
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si el objeto que entró es el jugador
        if (other.CompareTag("Player"))
        {
            // Desactivar los componentes
            if (boxCollider != null)
                boxCollider.enabled = false;

            if (spriteRenderer != null)
                spriteRenderer.enabled = false;
        }
    }
}
