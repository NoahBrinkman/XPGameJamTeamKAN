using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DirectionsData", order = 1)]
public class DirectionsData : ScriptableObject
{
    public float minAngle;
    public float maxAngle;
}
