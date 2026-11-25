using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public float velocidad;
    public float fuerzaSalto;
    public float distanciaSuelo;
    public Vector2 tamanoCajaSuelo;
    public LayerMask capaSuelo;
    public int vidaMax;

    Vector3 movimiento;
    Rigidbody2D cuerpo;
    Animator anim;
    bool saltando;
    int vidaRestante;
    Vector2 knockBack;
    bool muerto;

    void Start()
    {
        movimiento = new Vector3();
        knockBack = Vector2.zero;
        vidaRestante = vidaMax;
        cuerpo = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (muerto) return;   // estadno muerto no se mueve ni ataca

        LeerMovimiento();
        LeerSalto();
        LeerAtaque();
        RotarPersonaje();
        ActualizarAnim();
    }

    void FixedUpdate()
    {
        if (muerto)
        {
            cuerpo.velocity = new Vector2(0, cuerpo.velocity.y);
            return;
        }

        cuerpo.velocity = new Vector2(movimiento.x * velocidad, cuerpo.velocity.y);
        if (saltando)
        {
            cuerpo.AddForce(Vector2.up * fuerzaSalto);
            saltando = false;
        }
        if (knockBack != Vector2.zero)
        {
            cuerpo.AddForce(knockBack);
            knockBack = Vector2.zero;
        }
    }

    void LeerMovimiento()
    {
        if (Input.GetKey(KeyCode.A))
        {
            movimiento = Vector3.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movimiento = Vector3.right;
        }
        else
        {
            movimiento = new Vector3();
        }
    }

    bool EstaEnElSuelo()
    {
        RaycastHit2D rc = Physics2D.BoxCast(
            transform.position, tamanoCajaSuelo, 0, Vector2.down, distanciaSuelo, capaSuelo);
        if (rc)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void LeerSalto()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !saltando && EstaEnElSuelo())
        {
            saltando = true;
        }
    }

    // ataca con clic izquierdo y activa el trigger Atacando del Animator
    void LeerAtaque()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Atacando");
        }
    }

    void RotarPersonaje()
    {
        if (movimiento.x < 0)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else if (movimiento.x > 0)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }

    void ActualizarAnim()
    {
        if (movimiento.x == 0)
        {
            anim.SetBool("Moviendo", false);
        }
        else
        {
            anim.SetBool("Moviendo", true);
        }
    }

    void OnDrawGizmos()
    {
        if (EstaEnElSuelo())
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireCube(transform.position + Vector3.down * distanciaSuelo, tamanoCajaSuelo);
        Gizmos.color = Color.white;
    }

    // recibe danno lo llama el lobo en el evento golpear
    public void RecibeDanno(int cuanto, Vector2 posEnemigo)
    {
        if (muerto) return;

        vidaRestante -= cuanto;
        Debug.Log("Ouch!! Vida restante: " + vidaRestante);

        Vector2 direccionAtaque = ((Vector2)transform.position) - posEnemigo;
        direccionAtaque.Normalize();
        knockBack = direccionAtaque * 250 + 100 * Vector2.up;

        if (vidaRestante <= 0)
        {
            muerto = true;
            movimiento = Vector3.zero;
            anim.SetBool("Moviendo", false);
            anim.SetTrigger("Muerte");
        }
    }

    // al morir lo llama el evento de animación "Desaparecer" al final de Muere.
    // Se podría hacer una pantalla de derrota¿?
    public void Desaparecer()
    {
        Destroy(gameObject);
    }
}