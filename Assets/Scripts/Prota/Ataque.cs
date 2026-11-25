using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ataque : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject detectado = collision.gameObject;
        IGolpeable golpeable = detectado.GetComponent<IGolpeable>();
        if (golpeable != null)
        {
            golpeable.RecibeGolpe(1);
        }
    }
}