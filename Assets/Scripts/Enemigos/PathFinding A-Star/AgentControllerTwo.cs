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

    public List<Node> RunAStar(EnemyControllerTwo enemy)
    {
        var start = GetNearNode(enemy.transform.position);
        if (start == null) return new List<Node>();
        var outOfSightNodes = GetNodesOutOfSight(enemy.transform.position, radius, maskObs);
        if (outOfSightNodes.Count == 0) return new List<Node>();
        target = GetFurthestNode(outOfSightNodes, enemy.transform.position);
        return AStar.Run(start, GetConnections, IsSatiesfies, GetCost, Heuristic);
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
        float multiplierTrap = 200;
        cost += Vector3.Distance(parent.transform.position, child.transform.position) * multiplierDistance;
        return cost;
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

    List<Node> GetConnections(Node current)
    {
        return current.neightbourds;
    }
    public List<Node> GetNodesOutOfSight(Vector3 position, float range, LayerMask maskObs)
    {
        var nodes = Physics.OverlapSphere(position, range, maskNodes);
        var outOfSightNodes = new List<Node>();
        foreach (var nodeCollider in nodes) {
            var node = nodeCollider.GetComponent<Node>();
            var dirToNode = node.transform.position - position;
            if (!Physics.Raycast(position, dirToNode.normalized, dirToNode.magnitude, maskObs)) outOfSightNodes.Add(node);
        }
        return outOfSightNodes;
    }
    private Node GetFurthestNode(List<Node> nodes, Vector3 position) {
        Node furthestNode = null;
        float maxDistance = 0;
        foreach (var node in nodes) {
            float distance = Vector3.Distance(node.transform.position, position);
            if (distance > maxDistance) {
                maxDistance = distance;
                furthestNode = node;
            }
            return furthestNode;
        }
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
}
