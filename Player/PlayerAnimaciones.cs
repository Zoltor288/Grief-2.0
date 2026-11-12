/**********************************************************************************************************************
SISTEMA: Player

RESPONSABILIDAD:
-Controla las animaciones del PLAYER

DEPENDENCIAS:
-PlayerEnSuelo
**********************************************************************************************************************/

using System.Collections;
using UnityEngine;
namespace Player
{
    public class PlayerAnimaciones : MonoBehaviour
    {
        #region Propiedades     

        // REFERENCIAS 
        [SerializeField] AnimationClip _PlayerMuerte;
        [SerializeField] AnimationClip _PlayerDanyo;
        [SerializeField] AnimationClip _PlayerGolpe;

        Rigidbody2D _Rigidbody2D;
        PlayerJump _PlayerJump;
        Animator _Animator;
        DashHaciaEnemigo _DashHaciaEnemigo;

        // valor
        float offsetLimpiarVelocidadX = 0.01f;
        float offsetLimpiarVelocidadY = 0.01f;
        private int velocidadX_Hash, enSuelo_Hash, enTecho_Hash, danyo_Hash, muerte_Hash, velocidadY_Hash, sueloTrasSaltoSuelo_Hash, reiniciar_Hash, golpe_Hash, dash_Hash,dashEnemy_Hash, SwapGravity_Hash;
        
        
        //int inmunidad_INT;
        #endregion

        #region MonoBehaviour  
        void Awake()
        {
            _Rigidbody2D = GetComponent<Rigidbody2D>();
            _PlayerJump = GetComponent<PlayerJump>();
            _Animator = GetComponentInChildren<Animator>();
            _DashHaciaEnemigo = GetComponent<DashHaciaEnemigo>();

            // Coger los valores enteros de los parámetros tipo string. Mejora el rendimiento.
            velocidadX_Hash = Animator.StringToHash("VelocidadX");
            velocidadY_Hash = Animator.StringToHash("VelocidadY");
            enSuelo_Hash = Animator.StringToHash("EnSuelo");
            enTecho_Hash = Animator.StringToHash("EnTecho");
            muerte_Hash = Animator.StringToHash("Muerte");
            sueloTrasSaltoSuelo_Hash = Animator.StringToHash("SueloTrasSaltoSuelo");
            SwapGravity_Hash = Animator.StringToHash("SwapGravity");
            reiniciar_Hash = Animator.StringToHash("Reiniciar");
            danyo_Hash = Animator.StringToHash("Danyo");
            golpe_Hash = Animator.StringToHash("Golpe");
            dash_Hash = Animator.StringToHash("Dash");
            dashEnemy_Hash = Animator.StringToHash("DashEnemy");

        }

        private void Update()
        {
            IdleAndarCorrerSaltar();
            DashEnemigo();
        }
        #endregion

        public void IdleAndarCorrerSaltar()
        {
            _Animator.SetFloat(velocidadY_Hash, _Rigidbody2D.linearVelocity.y);
            _Animator.SetBool(enSuelo_Hash, _PlayerJump.enSuelo);
            _Animator.SetBool(enTecho_Hash, _PlayerJump.enTecho);
        }

        public void DashEnemigo()
        {
            _Animator.SetBool(dashEnemy_Hash, _DashHaciaEnemigo.isDashing);
        }
        public void TocarSueloTrasSaltoSuelo()
        {
            _Animator.SetTrigger(sueloTrasSaltoSuelo_Hash);
        }
        public void SwapGravity()
        {
            _Animator.SetFloat(SwapGravity_Hash, _Rigidbody2D.gravityScale);
        }
       
        public void Dash()
        {
            _Animator.SetTrigger(dash_Hash);
            
        }
        // -----------------------------------------
        public IEnumerator IE_Muerte()
        {
            _Animator.SetTrigger(muerte_Hash);
            yield return new WaitForSeconds(_PlayerMuerte.length);
        }
        // -----------------------------------------
        

        public IEnumerator IE_Golpe()
        {
            _Animator.SetTrigger(golpe_Hash);
            yield return new WaitForSeconds(_PlayerGolpe.length);
        }
        // -----------------------------------------
        public void Reiniciar()
        {
            _Animator.SetFloat(velocidadX_Hash, 0f);
            _Animator.SetFloat(velocidadY_Hash, 0f);
            _Animator.SetBool(enSuelo_Hash, false);
            _Animator.SetTrigger(muerte_Hash);
            _Animator.SetTrigger(sueloTrasSaltoSuelo_Hash);
            _Animator.SetFloat(SwapGravity_Hash, _Rigidbody2D.gravityScale);
            _Animator.SetTrigger(reiniciar_Hash);
        }
        
    }
}