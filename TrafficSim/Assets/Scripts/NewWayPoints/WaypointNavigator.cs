using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNavigator : MonoBehaviour
{
    // Se instancia un control del carrito y un waypoint
    CharacterNavigationController controller;
    public Waypoint currentWaypoint;

    // Este valor detecta si tiene un waypoint asignado o no
    int direction;

    // Despertamos el controlador
    private void Awake()
    {
        controller = GetComponent<CharacterNavigationController>();
    }

    // Se asigna un waypoint en el inicio y se asigna una direccion aleatoria
    void Start()
    {
        direction = Mathf.RoundToInt(Random.Range(0f, 1f));

        controller.SetDestination(currentWaypoint.GetPosition());
    }

    
    void Update()
    {
        // Cuando el carro llega al waypoint inicial
        if (controller.reachedDestination)
        {
            bool shouldBranch = false;

            // Checa si hay posibles desviaciones a partir de ese waypoint y decide si se desvia o no aleatoriamente
            if(currentWaypoint.branches != null && currentWaypoint.branches.Count > 0)
            {
                shouldBranch = Random.Range(0f, 1f) <= currentWaypoint.branchRatio ? true : false;
            }
            // Si se desvia, escoge aleatoriamente cual desviacion tomar
            if(shouldBranch)
            {
                currentWaypoint = currentWaypoint.branches[Random.Range(0, currentWaypoint.branches.Count - 1)];
            }
            // Si no, sigue con el siguiente waypoint del waypoint actual
            else
            {
                // Estos ifs hacen que no se quede en un Dead End en el mapa y siga moviendose el carro de un waypoint a otro   
                if (direction == 0)
                {
                    if (currentWaypoint.nextWaypoint != null)
                    {
                        currentWaypoint = currentWaypoint.nextWaypoint;
                    }
                    else
                    {
                        currentWaypoint = currentWaypoint.previousWaypoint;
                        direction = 1;
                    }
                }
                else if (direction == 1)
                {
                    if (currentWaypoint.previousWaypoint != null)
                    {
                        currentWaypoint = currentWaypoint.previousWaypoint;
                    }
                    else
                    {
                        currentWaypoint = currentWaypoint.nextWaypoint;
                        direction = 0;
                    }
                }
            }
            // Cambia el waypoint al siguiente, y asigna la posicion del siguiente waypoint como el destino del carro
            currentWaypoint = currentWaypoint.nextWaypoint;
            controller.SetDestination(currentWaypoint.GetPosition());
        }
    }
}
