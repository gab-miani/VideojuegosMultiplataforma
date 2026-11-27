using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nivel : MonoBehaviour
{
    public Transform destinoJugador;      // InicioNivel2, o un punto en PantallaVictoria
    public bool congelarJugador = false;  // true en victoria/derrota: el jugador ya no se mueve

    bool usada;

    void OnTriggerEnter2D(Collider2D other)
    {
        MovimientoJugador jugador = other.GetComponentInParent<MovimientoJugador>();
        if (jugador != null) Activar(jugador);
    }

    public void Activar(MovimientoJugador jugador)
    {
        if (usada) return;
        usada = true;

        // 1. Llevar al jugador
        Rigidbody2D rb = jugador.GetComponent<Rigidbody2D>();
        if (rb != null) rb.velocity = Vector2.zero;
        jugador.transform.position = new Vector3(
            destinoJugador.position.x, destinoJugador.position.y, jugador.transform.position.z);

        // 3. En victoria/derrota, dejar al jugador quieto
        if (congelarJugador)
        {
            jugador.enabled = false;
            Animator a = jugador.GetComponent<Animator>();
            if (a != null) a.SetBool("Moviendo", false);
            if (rb != null) rb.simulated = false;
        }
    }
}