using System.Collections;
using Player;
using UnityEngine;

public class PlayerMovHoriz : MonoBehaviour
{
    
    [SerializeField][Range(1f, 30f)] private float maxSpeed = 8f;
    [SerializeField][Range(0.5f, 5f)] private float accelerationTime = 2f;

    
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask capaPared;
    [SerializeField] private float collisionResetDelay = 0.1f;
    [SerializeField] private Vector3 dimensionesControlador = new Vector3(0.5f, 0.16f, 0);
    [SerializeField] private float distanciaControladorMuerte = 0.08f;
    [SerializeField] private Transform puntoDeteccionMuerte;

    private Rigidbody2D rb;
    private float currentSpeed;
    private bool isAccelerating = true;
    private Coroutine accelerationRoutine;
    [SerializeField] private bool canMove = true;
    [SerializeField] public bool Muerte;
    public bool CanMove => canMove;

    [SerializeField] private float deathAnimationDelay = 1f;

    private bool isDead = false;
    private PlayerAnimaciones playerAnimaciones;
    private DashHaciaEnemigo dashScript;
    [SerializeField] private GameObject deathCanvas;
    [SerializeField] private AudioClip _AUDMuerte;
    [SerializeField] private AudioClip _AUDNegativeMuerte;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        playerAnimaciones = GetComponent<PlayerAnimaciones>();
        dashScript = GetComponent<DashHaciaEnemigo>();
    }

    private void Start()
    {
        StartAcceleration();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocity.y);
        }

        // Detección del suelo - Solo si no estamos en dash
        if (dashScript == null || !dashScript.isDashing)
        {
            Vector2 puntoDeteccion = (puntoDeteccionMuerte != null)
                ? puntoDeteccionMuerte.position
                : new Vector2(transform.position.x, transform.position.y - distanciaControladorMuerte);

            Muerte = Physics2D.OverlapBox(puntoDeteccion, dimensionesControlador, 0f, capaPared);

            // Manejar la muerte si se detecta
            if (Muerte && !isDead)
            {
                HandleDeath();
            }
        }
        else
        {
            Muerte = false; // Resetear detección durante el dash
        }
    }

    // Métodos para modificar "canMove"
    public void EnableMovement() => canMove = true;
    public void DisableMovement() => canMove = false;

    public void StartAcceleration()
    {
        if (!canMove) return;

        if (accelerationRoutine != null)
        {
            StopCoroutine(accelerationRoutine);
        }
        accelerationRoutine = StartCoroutine(AcceleratePlayer());
    }

    private IEnumerator AcceleratePlayer()
    {
        float timer = 0f;
        currentSpeed = 0f;
        isAccelerating = true;

        while (timer < accelerationTime)
        {
            currentSpeed = Mathf.SmoothStep(0f, maxSpeed, timer / accelerationTime);
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        currentSpeed = maxSpeed;
        isAccelerating = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((obstacleLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            HandleCollisionReset();
        }
    }

    private void HandleCollisionReset()
    {
        DisableMovement();
        rb.linearVelocity = Vector2.zero;
        currentSpeed = 0f;

        if (accelerationRoutine != null)
        {
            StopCoroutine(accelerationRoutine);
        }

        StartCoroutine(ResetMovementAfterDelay());
    }

    private IEnumerator ResetMovementAfterDelay()
    {
        yield return new WaitForSeconds(collisionResetDelay);
        EnableMovement();
        StartAcceleration();
    }

    public bool IsAtMaxSpeed() => !isAccelerating;

    private void HandleDeath()
    {
        isDead = true;
        Muerte = true; // Asegurar consistencia

        // Congelar completamente al jugador
        rb.linearVelocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        canMove = false;

        // Detener todas las corutinas de movimiento
        if (accelerationRoutine != null)
        {
            StopCoroutine(accelerationRoutine);
        }

        // Activar canvas de muerte
        if (deathCanvas != null)
        {
            deathCanvas.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Death Canvas no asignado en el Inspector");
        }

        // Opcional: Activar animación de muerte
        if (Muerte && rb.gravityScale > -0.1)
        {
            ControladorSonido.Instance.EjecutarSonido(_AUDMuerte);
            StartCoroutine(playerAnimaciones.IE_Muerte());
        }

        if (Muerte && rb.gravityScale < 0)
        {
            ControladorSonido.Instance.EjecutarSonido(_AUDNegativeMuerte);
            StartCoroutine(playerAnimaciones.IE_Muerte());
        }
    }

    private void OnDrawGizmos()
    {
        if (puntoDeteccionMuerte != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(puntoDeteccionMuerte.position, dimensionesControlador);
        }
    }
}