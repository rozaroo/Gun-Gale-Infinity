using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentControllerTwo : MonoBehaviour
{
    public EnemyControllerTwo enemy;
    public float radius = 3;
    public LayerMask maskObs;
    public Vector3 target;
    public MyGrid myGrid;

    /*public List<Node> RunAStar()
    {
        var start = GetNearNode(enemy.transform.position);
        if (start == null) return new List<Node>();
        return AStar.Run(start, GetConnections, IsSatiesfies, GetCost, Heuristic);
    }*/
    public List<Vector3> RunAStarPlusVector()
    {
        var start = myGrid.GetPosInGrid(enemy.transform.position);
        if (start == null) return new List<Vector3>();
        return AStar.Run(start, GetConnections, IsSatiesfies, GetCost, Heuristic, 5000);
    }
    float Heuristic(Vector3 current)
    {
        return Vector3.Distance(current, target);
    }
    float GetCost(Vector3 parent, Vector3 child)
    {
        return Vector3.Distance(parent, child);
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
                if (myGrid.IsRightPos(point))
                {
                    connections.Add(point);
                }
            }
        }
        return connections;
    }
    bool IsSatiesfies(Vector3 current)
    {
        return Vector3.Distance(current, target) < 1.0f;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(enemy.transform.position, radius);
    }
}
