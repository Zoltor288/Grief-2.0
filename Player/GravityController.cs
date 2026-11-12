using Player;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerJump playerJump;
    private PlayerAnimaciones _playerAnimaciones;
    private bool gravityNormal = true;

    [SerializeField] private KeyRebinder keyRebinder;
    [SerializeField] private AudioClip _AUDGravity;

    public GameObject objeto1;  
    public GameObject objeto2;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerJump = GetComponent<PlayerJump>();
        _playerAnimaciones = GetComponent<PlayerAnimaciones>();

        if (playerJump == null)
        {
            Debug.LogError("Componente PlayerJump no encontrado en el objeto");
        }
    }

    void Update()
    {
        KeyCode gravityKey = KeyRebinder.Instance.GetKey("Fire3");

        if (Input.GetKeyDown(gravityKey))
        {
            // Solo permite cambiar gravedad si está en suelo o techo
            if (playerJump != null && (playerJump.enSuelo || playerJump.enTecho))
            {
                ToggleGravity();
                ControladorSonido.Instance.EjecutarSonido(_AUDGravity);
                bool estadoActualObjeto1 = objeto1.activeSelf;
                objeto1.SetActive(!estadoActualObjeto1);
                objeto2.SetActive(estadoActualObjeto1);
            }
        }
    }

    public void ToggleGravity()
    {
        if (rb == null) return;

        rb.gravityScale *= -1;
        gravityNormal = !gravityNormal;

        _playerAnimaciones.SwapGravity();

        // Rotación opcional (descomentar si se necesita)
        //transform.Rotate(180, 0, 0);
    }
}