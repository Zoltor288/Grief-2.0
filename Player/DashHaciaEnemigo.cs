using UnityEngine;
using System.Collections;

public class DashHaciaEnemigo : MonoBehaviour
{
    public float velocidad = 10f;
    public float rangoActivacion = 5f;

    private BoxCollider2D BoxCollider2D;
    private Rigidbody2D rb;
    private PlayerMovHoriz playerMov;
    public bool isDashing = false;
    private Vector2 targetPosition;
    private float gravedadOriginal;
    private RigidbodyConstraints2D constraintsOriginales;
    [SerializeField] private AudioClip _AUDDashE;



    void Start()
    {
        BoxCollider2D = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        gravedadOriginal = rb.gravityScale;
        constraintsOriginales = rb.constraints;
        playerMov = GetComponent<PlayerMovHoriz>();
    }

    void Update()
    {
        KeyCode enemyDashKey = KeyRebinder.Instance.GetKey("Fire4");
        KeyCode gravityKey = KeyRebinder.Instance.GetKey("Fire3");

        if (Input.GetKeyDown(enemyDashKey) && !isDashing)
        {
            Collider2D[] enemigos = Physics2D.OverlapCircleAll(transform.position, rangoActivacion);
            Transform enemigoCercano = EncontrarEnemigoMasCercano(enemigos);

            if (enemigoCercano != null)
            {
                targetPosition = enemigoCercano.position;
                StartCoroutine(RealizarDash());
            }
        }

        if (Input.GetKeyDown(gravityKey))
        {
            gravedadOriginal = gravedadOriginal * -1;
        }
    }

    Transform EncontrarEnemigoMasCercano(Collider2D[] enemigos)
    {
        Transform masCercano = null;
        float distanciaMinima = Mathf.Infinity;

        foreach (Collider2D enemigo in enemigos)
        {
            if (enemigo.CompareTag("Enemigo"))
            {
                float distanciaActual = Vector2.Distance(transform.position, enemigo.transform.position);
                if (distanciaActual < distanciaMinima)
                {
                    distanciaMinima = distanciaActual;
                    masCercano = enemigo.transform;
                }
            }
        }
        return masCercano;
    }

    IEnumerator RealizarDash()
    {
        isDashing = true;
        BoxCollider2D.enabled = false;
        ControladorSonido.Instance.EjecutarSonido(_AUDDashE);

        if (playerMov != null) playerMov.Muerte = false;

        // Configuración corregida del Rigidbody
        rb.linearVelocity = Vector2.zero; 
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; 

        while (Vector2.Distance(transform.position, targetPosition) > 0.1f)
        {
            rb.MovePosition(Vector2.MoveTowards(
                rb.position,
                targetPosition,
                velocidad * Time.fixedDeltaTime
            ));
            yield return new WaitForFixedUpdate();
        }

        // Restauración de valores
        BoxCollider2D.enabled = true;
        rb.gravityScale = gravedadOriginal;
        rb.constraints = constraintsOriginales;
        isDashing = false;
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoActivacion);
    }
}