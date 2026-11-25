using UnityEngine;

public class MovimientoCamara : MonoBehaviour

{
    public Transform player;
    // Start is called before the first frame update
    void Start()
{

}

// Update is called once per frame
void FixedUpdate()
{
    Vector3 posDestino = player.position;
    Vector3 posicionVieja = transform.position;
    Vector3 posNueva = new Vector3(
        Mathf.Lerp(posicionVieja.x, posDestino.x, 5 * Time.fixedDeltaTime),
        posicionVieja.y, //Mathf.Lerp(posicionVieja.y, posDestino.y, 5 * Time.deltaTime),
        posicionVieja.z
    );
    transform.position = posNueva;
}
}

