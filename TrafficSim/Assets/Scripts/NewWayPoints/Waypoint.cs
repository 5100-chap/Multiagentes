using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    // Instanciamos 2 tipos de waypoints
    public Waypoint previousWaypoint;
    public Waypoint nextWaypoint;

    // Asignamos el ancho del waypoint
    [Range(0f, 5f)]
    public float width = 1f;

    // Creamos una lista de waypoints que seran los posibles branches
    public List<Waypoint> branches = new List<Waypoint>();

    // Ponemos en 0.5 el valor de branchRatio para ser usado como toma de decisiones en WaypointNavigator.cs
    [Range(0f, 1f)]
    public float branchRatio = 0.5f;

    // Regresa la posicion del vector 
    public Vector3 GetPosition()
    {
        Vector3 minBound = transform.position + transform.right * width / 2f;
        Vector3 maxBound = transform.position - transform.right * width / 2f;

        return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
    }

}
