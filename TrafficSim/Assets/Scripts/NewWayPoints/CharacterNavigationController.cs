using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNavigationController : MonoBehaviour
{
    // el carro ocupa todos estos parametros para poder moverse y saber donde esta y a donde va en el mapa
    public float movementSpeed;
    public float rotationSpeed = 120f;
    public float stopDistance = 0.2f;
    Vector3 lastPosition;
    Vector3 velocity;
    public Vector3 destination;
    public bool reachedDestination = false;
    public Animator animator;

    /*
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    */

    // En cada frame se actualiza la posicion de carro con su destino
    void Update()
    {
        // Si el carro no ha llegado a su destino, actualiza la direccion del carro para apuntar al siguiente waypoint
        if (transform.position != destination)
        {
            Vector3 destinationDirection = destination - transform.position;
            destinationDirection.y = 0;

            float destinationDistance = destinationDirection.magnitude;
            
            // Se asigna un movimiento aleatoria para el carro que cambia constantemente
            movementSpeed = Random.Range(0.9f, 10f);

            // Si el carrito esta aun lejos del umbral, 
            if (destinationDistance >= stopDistance)
            {
                // Se reconoce que aun no llega al destino y se rota hacia el punto destino
                reachedDestination = false;
                Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);

                // Se actualiza la velocidad (un vector) para hacer el cambio visuales del carro
                velocity = (transform.position - lastPosition) / Time.deltaTime;
                velocity.y = 0;
                var velocityMagnitude = velocity.magnitude;
                velocity = velocity.normalized;
                // Para indicar el movimiento lateral y el movimiento horizontal
                var fwdDotProduct = Vector3.Dot(transform.forward, velocity);
                var rightDotProduct = Vector3.Dot(transform.right, velocity);


                //animator.SetFloat("MovementX", rightDotProduct);
                //animator.SetFloat("MovementSpeed", -fwdDotProduct);

            }
            // Si esta dentro del umbral, regresa que ya llego a su destino
            else
            {
                reachedDestination = true;
            }


        }
        // Si la posicion actual es la misma que la del destino, regresa que ya llego al punto.
        else
        {
            reachedDestination = true;

        }

        
    }

    // Asigna un waypoint especifico en el mapa
    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        reachedDestination = false;
    }
}
