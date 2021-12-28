using UnityEngine;

[CreateAssetMenu(fileName = "Boat", menuName = "Entity/Boat")]
public class BoatType : ScriptableObject
{
    public GameObject prefab;
    public int seatCapacity;
    public float thrust = 1200f;
    public float turnSpeed = 20f;
}
