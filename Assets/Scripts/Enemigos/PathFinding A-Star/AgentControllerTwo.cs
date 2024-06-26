using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentControllerTwo : MonoBehaviour
{
    public EnemyControllerTwo enemy;
    public float radius = 3;
    public LayerMask maskNodes;
    public LayerMask maskObs;
    public Node target;
    public Node start;
    public List<Objetive> objetives; 
    public MyGrid myGrid;

    public List<Vector3> RunAStarPlusVector()
    {
        Vector3 start = myGrid.GetPosInGrid(enemy.transform.position);
        Vector3 farthestPoint = GetFarthestPointOutOfView(start);
        if (farthestPoint == Vector3.zero) return new List<Vector3>();

        List<Vector3> path = AStar.Run(start, GetConnections, (current) => IsSatiesfies(current, farthestPoint), GetCost, (current) => Heuristic(current, farthestPoint), 5000);
        return path;
    }
    float Heuristic(Node current)
    {
        float heuristic = 0;
        float multiplierDistance = 1;
        heuristic += Vector3.Distance(current.transform.position, target.transform.position) * multiplierDistance;
        return heuristic;
    }
    float GetCost(Node parent, Node child)
    {
        float cost = 0;
        float multiplierDistance = 1;
        cost += Vector3.Distance(parent.transform.position, child.transform.position) * multiplierDistance;
        return cost;
    }
   
    List<Node> GetConnections(Node current)
    {
        return current.neightbourds;
    }
    bool IsSatiesfies(Node current)
    {
        return current == target;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(enemy.transform.position, radius);
    }
    //Vector3
    float GetCost(Vector3 parent, Vector3 child)
    {
        float cost = 0;
        float multiplierDistance = 1;
        cost += Vector3.Distance(parent, child) * multiplierDistance;
        return cost;
    }

    List<Vector3> GetConnections(Vector3 current)
    {
        var connections = new List<Vector3>();

        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (z == 0 && x == 0) continue;
                Vector3 point = myGrid.GetPosInGrid(new Vector3(current.x + x, current.y, current.z + z));
                Debug.Log(point + "  " + myGrid.IsRightPos(point));
                if (myGrid.IsRightPos(point)) connections.Add(point);
            }
        }
        return connections;
    }
    bool IsSatiesfies(Vector3 current, Vector3 targetPosition)
    {
        return Vector3.Distance(current, targetPosition) < 2 && InView(current, targetPosition);
    }
    Node GetNearNode(Vector3 pos)
    {
        var nodes = Physics.OverlapSphere(pos, radius, maskNodes);
        Node nearNode = null;
        float nearDistance = 0;
        for (int i = 0; i < nodes.Length; i++)
        {
            var currentNode = nodes[i];
            var dir = currentNode.transform.position - pos;
            float currentDistance = dir.magnitude;
            if (nearNode == null || currentDistance < nearDistance)
            {
                if (!Physics.Raycast(pos, dir.normalized, currentDistance, maskObs))
                {
                    nearNode = currentNode.GetComponent<Node>();
                    nearDistance = currentDistance;
                }
            }
        }
        return nearNode;
    }
    float Heuristic(Vector3 current,Vector3 targetPosition)
    {
        float heuristic = 0;
        float multiplierDistance = 1;
        heuristic += Vector3.Distance(current, targetPosition) * multiplierDistance;
        return heuristic;
    }
    bool InView(Vector3 a, Vector3 b)
    {
        //a->b  b-a
        Vector3 dir = b - a;
        return !Physics.Raycast(a, dir.normalized, dir.magnitude, enemy.maskObs);
    }
    Vector3 GetFarthestPointOutOfView(Vector3 start)
    {
        Vector3 farthestPoint = Vector3.zero;
        float maxDistance = float.MinValue;
        foreach (var obj in objetives) 
        {
            if (enemy.CheckView(objetives.transform) == false)
            {
                float distance = Vector3.Distance(start, objPosition);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    farthestPoint = objPosition;
                }
            }
        }
        
        return farthestPoint;
    }
}
