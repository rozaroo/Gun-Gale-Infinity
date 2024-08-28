using UnityEngine;

[CreateAssetMenu(fileName = "PlayerValues", menuName = "ScriptableObjects/PlayerValues", order = 2)]
public class PlayerValues : ScriptableObject
{
    public float[] Speed;
    public float[] MaxHealth;
    public float[] CamRotSpeed;
    public float[] MinAngle;
    public float[] MaxAngle;
    public float[] CameraSpeed;
    public GameObject[] ItemPrefab;
}
