using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("Configuración Básica de Salto")]
    [SerializeField] private float fuerzaSalto = 8f;
    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] private LayerMask capaTecho;
    [SerializeField] private Transform puntoDeteccionSuelo;
    [SerializeField] private Transform puntoDeteccionTecho;
    [SerializeField] private float distanciaControladorSuelo = 0.08f;
    [SerializeField] private float distanciaControladorTecho = 0.08f;
    [SerializeField] private Vector3 dimensionesControlador = new Vector3(0.5f, 0.16f, 0);
    [SerializeField] private float fuerzaCaidaRapida = 15f;

    [Header("Doble Salto")]
    [SerializeField] private int maxSaltosEnAire = 2;
    private int saltosRestantes;

    [Header("Doble Salto techo")]
    [SerializeField] private int maxSaltosEnAireTecho = 2;
    private int saltosRestantesTecho;

    private bool quiereSaltar;
    private bool quiereCaidaRapida;
    private Rigidbody2D _Rigidbody2D;
    private Animator _Animator;
    [SerializeField] public bool enSuelo;
    [SerializeField] public bool enTecho;
    [SerializeField] private AudioClip _AUDJump;
    [SerializeField] private AudioClip _AUDNegativeJump;
    [SerializeField] private AudioClip _AUDCaidaRapida;

    private void Awake()
    {
        _Rigidbody2D = GetComponent<Rigidbody2D>();
        

    }

    private void Start()
    {
        saltosRestantes = maxSaltosEnAire;
        saltosRestantesTecho = maxSaltosEnAireTecho;
    }

    private void Update()
    {
        // Verificar si KeyRebinder está inicializado
        if (KeyRebinder.Instance != null)
        {
            // Obtener teclas configuradas
            KeyCode jumpKey = KeyRebinder.Instance.GetKey("Jump");
            KeyCode fireKey = KeyRebinder.Instance.GetKey("Fire1");

            quiereSaltar |= Input.GetKeyDown(jumpKey);
            quiereCaidaRapida |= Input.GetKeyDown(fireKey);
        }
        else
        {
            // Usar valores por defecto si KeyRebinder no está listo
            quiereSaltar |= Input.GetKeyDown(KeyCode.Space);
            quiereCaidaRapida |= Input.GetKeyDown(KeyCode.LeftControl);
        }
    }

    private void FixedUpdate()
    {
        // Detección del suelo
        Vector2 puntoDeteccion = (puntoDeteccionSuelo != null)
            ? puntoDeteccionSuelo.position
            : new Vector2(transform.position.x, transform.position.y - distanciaControladorSuelo);
        enSuelo = Physics2D.OverlapBox(puntoDeteccion, dimensionesControlador, 0f, capaSuelo);

        // Detección del techo
        Vector2 puntoDeteccionArriba = (puntoDeteccionTecho != null)
            ? puntoDeteccionTecho.position
            : new Vector2(transform.position.x, transform.position.y - distanciaControladorTecho);
        enTecho = Physics2D.OverlapBox(puntoDeteccionArriba, dimensionesControlador, 0f, capaTecho);

        // Restablecer saltos al tocar el suelo
        if (enSuelo && _Rigidbody2D.linearVelocity.y <= 0.01f)
        {
            saltosRestantes = maxSaltosEnAire;
            saltosRestantesTecho = maxSaltosEnAireTecho;
        }

        // Restablecer saltos al tocar el techo
        if (enTecho && _Rigidbody2D.linearVelocity.y <= 0.01f)
        {
            saltosRestantes = maxSaltosEnAire;
            saltosRestantesTecho = maxSaltosEnAireTecho;
        }

        // Caída rápida
        if (quiereCaidaRapida && !enSuelo)
        {
            if (enSuelo)
            {
                // Salto desde suelo
                _Rigidbody2D.linearVelocity = new Vector2(_Rigidbody2D.linearVelocity.x, fuerzaSalto);
                saltosRestantes = maxSaltosEnAireTecho - 1;
                

            }
            // Activar solo si está en el techo y tiene usos restantes
            else if (saltosRestantesTecho > 0)
            {
                _Rigidbody2D.linearVelocity = new Vector2(_Rigidbody2D.linearVelocity.x, -fuerzaCaidaRapida);
                saltosRestantesTecho--; // Consume un uso  
                if (_Rigidbody2D.gravityScale < 0)
                {
                    ControladorSonido.Instance.EjecutarSonido(_AUDNegativeJump);
                }
            } 
        }

        

        // Lógica de salto
        if (quiereSaltar && !enTecho)
        {
            if (enSuelo)
            {
                // Salto desde suelo
                _Rigidbody2D.linearVelocity = new Vector2(_Rigidbody2D.linearVelocity.x, fuerzaSalto);
                saltosRestantes = maxSaltosEnAire - 1;
                if (_Rigidbody2D.gravityScale > -0.1)
                {
                    ControladorSonido.Instance.EjecutarSonido(_AUDJump);
                }
            }
            else if (saltosRestantes > 0)
            {
                // Doble salto desde aire
                _Rigidbody2D.linearVelocity = new Vector2(_Rigidbody2D.linearVelocity.x, fuerzaSalto);
                saltosRestantes--;
                
            }
        }

        quiereSaltar = false;
        quiereCaidaRapida = false;
    }


    private void OnDrawGizmos()
    {
        if (puntoDeteccionSuelo != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(puntoDeteccionSuelo.position, dimensionesControlador);
        }

        if (puntoDeteccionTecho != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(puntoDeteccionTecho.position, dimensionesControlador);
        }
    }
}
