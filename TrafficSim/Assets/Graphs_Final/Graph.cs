using System.Collections.Generic;
using UnityEngine;

public class Graph {
    List<Edge> edges = new List<Edge>();
    List<Node> nodes = new List<Node>();
    List<Node> pathList = new List<Node>();

    // Empty Graph
    public Graph() { }

    // Adds a Node the list of nodes and makes it traversable
    public void AddNode(GameObject id, bool removeRenderer = true, bool removeCollider = true, bool removeId = true) {
        Node node = new Node(id);
        nodes.Add(node);

        //remove colliders and mesh renderer
        if (removeCollider)
            GameObject.Destroy(id.GetComponent<Collider>());
        if (removeRenderer)
            GameObject.Destroy(id.GetComponent<Renderer>());
        if (removeId) {
            TextMesh[] textms = id.GetComponentsInChildren<TextMesh>() as TextMesh[];

            foreach (TextMesh tm in textms)
                GameObject.Destroy(tm.gameObject);
        }
    }

    // creates the edges of each node which are the links
    public void AddEdge(GameObject fromNode, GameObject toNode) {
        Node from = findNode(fromNode);
        Node to = findNode(toNode);

        if (from != null && to != null) {
            Edge e = new Edge(from, to);
            edges.Add(e);
            from.edgelist.Add(e);
        }
    }

    // finds the node with the given id
    Node findNode(GameObject id) {
        foreach (Node n in nodes) {
            if (n.getId() == id)
                return n;
        }
        return null;
    }

    // returns the path length
    public int getPathLength() {
        return pathList.Count;
    }

    // retuns the id of a waypoint or node
    public GameObject getPathPoint(int index) {
        return pathList[index].id;
    }

    // prints the path
    public void printPath() {
        foreach (Node n in pathList) {
            Debug.Log(n.id.name);
        }
    }

    // with the a* algorithm create the best path between 2 given waypoints
    public bool AStar(GameObject startId, GameObject endId) {
        Node start = findNode(startId);
        Node end = findNode(endId);

        // returns false if there is no end or start
        if (start == null || end == null) {
            return false;
        }

        // creates 2 lists (open and close)
        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();
        float tentative_g_score = 0;
        bool tentative_is_better;

        // initializes the variables g, h and f of the start node
        start.g = 0;
        start.h = distance(start, end);
        start.f = start.h;
        open.Add(start);

        // while the open list has waypoint, continues registering the lowest f
        while (open.Count > 0) {
            int i = lowestF(open);
            Node thisnode = open[i];
            if (thisnode.id == endId)  //path found
            {
                reconstructPath(start, end);
                return true;
            }

            // removes it from the open list and adds it to the closed list
            open.RemoveAt(i);
            closed.Add(thisnode);

            Node neighbour;
            // checks every link of the node registering and changes the g, f and h values
            foreach (Edge e in thisnode.edgelist) {
                neighbour = e.endNode;
                neighbour.g = thisnode.g + distance(thisnode, neighbour);

                if (closed.IndexOf(neighbour) > -1)
                    continue;

                tentative_g_score = thisnode.g + distance(thisnode, neighbour);

                if (open.IndexOf(neighbour) == -1) {
                    open.Add(neighbour);
                    tentative_is_better = true;
                } else if (tentative_g_score < neighbour.g) {
                    tentative_is_better = true;
                } else
                    tentative_is_better = false;

                if (tentative_is_better) {
                    neighbour.cameFrom = thisnode;
                    neighbour.g = tentative_g_score;
                    //neighbour.h = distance(thisnode,neighbour);
                    neighbour.h = distance(thisnode, end);
                    neighbour.f = neighbour.g + neighbour.h;
                }
            }

        }

        return false;
    }

    // build the path with the new nodes or waypoints
    public void reconstructPath(Node startId, Node endId) {
        pathList.Clear();
        pathList.Add(endId);

        var p = endId.cameFrom;
        while (p != startId && p != null) {
            pathList.Insert(0, p);
            p = p.cameFrom;
        }
        pathList.Insert(0, startId);
    }

    // calculates the magnitude distance between 2 nodes or waypoints
    float distance(Node a, Node b) {
        float dx = a.xPos - b.xPos;
        float dy = a.yPos - b.yPos;
        float dz = a.zPos - b.zPos;
        float dist = dx * dx + dy * dy + dz * dz;
        return (dist);
    }

    // calculates the lowest f in an array of nodes
    int lowestF(List<Node> l) {
        float lowestf = 0;
        int count = 0;
        int iteratorCount = 0;

        for (int i = 0; i < l.Count; i++) {
            if (i == 0) {
                lowestf = l[i].f;
                iteratorCount = count;
            } else if (l[i].f <= lowestf) {
                lowestf = l[i].f;
                iteratorCount = count;
            }
            count++;
        }
        return iteratorCount;
    }

    // draw the waypoints and paths
    public void debugDraw() {
        //draw edges
        for (int i = 0; i < edges.Count; i++) {
            Debug.DrawLine(edges[i].startNode.id.transform.position, edges[i].endNode.id.transform.position, Color.red);

        }
        //draw directions
        for (int i = 0; i < edges.Count; i++) {
            Vector3 to = (edges[i].startNode.id.transform.position - edges[i].endNode.id.transform.position) * 0.05f;
            Debug.DrawRay(edges[i].endNode.id.transform.position, to, Color.blue);
        }
    }
}