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

    Vector3 movimiento;
    Rigidbody2D cuerpo;
    Animator anim;
    bool saltando;
    int vidaRestante;

    // Start is called before the first frame update
    void Start()
    {
        movimiento = new Vector3();
        cuerpo = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        LeerMovimiento();
        LeerSalto();
        RotarPersonaje();
        ActualizarAnim();
    }
    void FixedUpdate()
    {
        cuerpo.velocity = new Vector2(movimiento.x * velocidad, cuerpo.velocity.y);
        if (saltando)
        {
            cuerpo.AddForce(Vector2.up * fuerzaSalto);
            saltando = false;
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

    public void RecibeDanno(int cuanto, Vector2 posEnemigo)
    {
        vidaRestante -= cuanto;
        Debug.Log("Ouch!! Vida restante: " + vidaRestante);
        if (vidaRestante <= 0) Destroy(gameObject);
        Vector2 direccionAtaque = ((Vector2)transform.position) - posEnemigo;
        direccionAtaque.Normalize();
    }

    void OnMouseDrag()
    {
        Vector3 mouseWP = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gameObject.transform.position = new Vector3(mouseWP.x, mouseWP.y, gameObject.transform.position.z);

    }


}
