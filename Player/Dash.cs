using System.Collections;
using Player;
using UnityEngine;

public class Dash : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    [SerializeField] private float fuerzaDash = 20f;
    [SerializeField] private float tiempoDash = 0.2f;
    [SerializeField] private float cooldownDash = 1f;
    

    private float _gravedadInicial;
    private bool _puedeHacerDash = true;
    private PlayerMovHoriz _playerMov;
    private PlayerAnimaciones _playerAnimaciones;
    [SerializeField] private AudioClip _AUDDash;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerMov = GetComponent<PlayerMovHoriz>();
        _playerAnimaciones = GetComponent<PlayerAnimaciones>();
    }

    private void Start()
    {
        _gravedadInicial = _rigidbody2D.gravityScale;
    }

    private void Update()
    {
        KeyCode dashKey = KeyRebinder.Instance.GetKey("Fire2");
        KeyCode gravityKey = KeyRebinder.Instance.GetKey("Fire3");

        if (_puedeHacerDash && Input.GetKeyDown(dashKey))
        {
            StartCoroutine(EjecutarDash());
        }

        if (Input.GetKeyDown(gravityKey))
        {
            _gravedadInicial = _gravedadInicial * -1;
        }
    }

    private IEnumerator EjecutarDash()
    {
        _puedeHacerDash = false;
        ControladorSonido.Instance.EjecutarSonido(_AUDDash);
        // Guardar estado original
        RigidbodyConstraints2D constraintsOriginales = _rigidbody2D.constraints;
        bool podiaMoverse = _playerMov.CanMove;

        // Desactivar movimiento usando los métodos públicos
        _playerMov.DisableMovement();
        _rigidbody2D.constraints |= RigidbodyConstraints2D.FreezePositionY;
        _rigidbody2D.gravityScale = 0;

        // Activa la animación del dash
        _playerAnimaciones.Dash();

        // Calcular dirección
        float direccion = transform.localScale.x > 0 ? 1 : -1;
        Vector2 velocidadDash = new Vector2(fuerzaDash * direccion, 0);
        _rigidbody2D.linearVelocity = velocidadDash;

        yield return new WaitForSeconds(tiempoDash);

        // Restaurar estado
        _rigidbody2D.gravityScale = _gravedadInicial;
        _rigidbody2D.constraints = constraintsOriginales;

        // Usar EnableMovement/DisableMovement en lugar de asignar CanMove directamente
        if (podiaMoverse)
        {
            _playerMov.EnableMovement();
        }

        yield return new WaitForSeconds(cooldownDash);
        _puedeHacerDash = true;
    }
}
