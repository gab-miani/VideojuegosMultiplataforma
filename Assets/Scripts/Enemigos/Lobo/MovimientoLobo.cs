using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoLobo : MonoBehaviour, IGolpeable
{
    public Vector2 tamannoAtaque;
    public float distanciaAtaque;
    public LayerMask capaJugador;
    public int vidaMax;

    Animator anim;
    SpriteRenderer sprite;
    GameObject aGolpear;
    int vida;
    int direccion;
    bool muerto;

    void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        vida = vidaMax;
        direccion = 1;
    }

    void Update()
    {
        if (muerto) return;

        Girar();

        if (DetectarJugador())
        {
            anim.SetBool("Atacar", true);
        }
        else
        {
            anim.SetBool("Atacar", false);
        }
    }

    // Girar el sprite según la dirección
    void Girar()
    {
        if (direccion >= 0)
        {
            sprite.flipX = false;
        }
        else
        {
            sprite.flipX = true;
        }
    }

    // DETECTAR: lanza una caja delante del lobo; si toca al jugador, devuelve true
    bool DetectarJugador()
    {
        RaycastHit2D rayo = Physics2D.BoxCast(
            transform.position, tamannoAtaque, 0,
            Vector2.right * direccion, distanciaAtaque, capaJugador);

        if (rayo)
        {
            aGolpear = rayo.rigidbody.gameObject;
            return true;
        }
        else
        {
            aGolpear = null;
            return false;
        }
    }

    // ATACAR: lo llama el evento de animación "Golpear" en el mordisco
    public void Golpear()
    {
        if (aGolpear != null)
        {
            aGolpear.GetComponent<MovimientoJugador>().RecibeDanno(1, transform.position);
        }
    }

    // RECIBIR DAÑO: lo llama el ataque del jugador
    public void RecibeGolpe(int danno)
    {
        if (muerto) return;

        vida -= danno;
        if (vida <= 0)
        {
            muerto = true;
            anim.SetBool("Atacar", false);
            anim.SetTrigger("Muerte");
        }
        else
        {
            anim.SetTrigger("Herido");
        }
    }

    // MORIR: lo llama el evento de animación "Desaparecer" al final de Muere
    public void Desaparecer()
    {
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(
            transform.position + Vector3.right * direccion * distanciaAtaque,
            tamannoAtaque);
    }
}