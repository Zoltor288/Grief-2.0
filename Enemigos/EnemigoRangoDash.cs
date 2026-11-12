using UnityEngine;

public class EnemigoRangoDash : MonoBehaviour
{
    public Sprite spriteNormal;     // Sprite cuando está fuera de rango
    public Sprite spriteEnRango;    // Sprite cuando está dentro del rango de dash
    public float rangoDeteccion = 5f; // Debe coincidir con el rango del jugador

    private Transform jugador;
    private SpriteRenderer spriteRenderer;
    private DashHaciaEnemigo dashJugador;

    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (jugador != null)
        {
            dashJugador = jugador.GetComponent<DashHaciaEnemigo>();
        }
    }

    void Update()
    {
        if (jugador == null || dashJugador == null) return;

        float distancia = Vector2.Distance(transform.position, jugador.position);

        // Cambia el sprite según si está dentro del rango de dash
        if (distancia <= rangoDeteccion)
        {
            spriteRenderer.sprite = spriteEnRango;
        }
        else
        {
            spriteRenderer.sprite = spriteNormal;
        }
    }

    // Opcional: Visualizar el rango en el editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
    }
}
