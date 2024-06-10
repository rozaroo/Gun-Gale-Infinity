using UnityEngine;

[CreateAssetMenu(fileName = "DropPrefabs",menuName = "ScriptableObjects/DropPrefabs",order = 3)]
public class DropPrefabs : ScriptableObject
{
    public GameObject[] prefabs;
}