using UnityEngine;
using System.Collections;

public class MoveToPoint : MonoBehaviour
{

    public Vector2 initialPosition;
    public Vector2 destination;

    public float duration;

    public float progress = 0;

    // Use this for initialization
    void Start()
    {
        progress = 0;
        DefineNewDestination(destination);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        progress += (Time.deltaTime / duration);

        Vector2 myPosition = transform.localPosition;
        transform.localPosition = Vector2.Lerp(initialPosition, destination, progress);
    }


    //Nous mettons ceci dans une fonction, afin de le réutiliser au cas où l'on devrait faire bouger un ennemi
    public void DefineNewDestination(Vector2 newDestination)
    {
        progress = 0;
        destination = newDestination;
        initialPosition = GetComponent<Rigidbody2D>().transform.position;
    }

}