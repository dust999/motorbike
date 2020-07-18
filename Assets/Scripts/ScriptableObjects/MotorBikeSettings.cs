using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/MotorBikeSettings", fileName = "MotorBikeSettings")]
public class MotorBikeSettings : ScriptableObject
{
    public float maxSpeed = 3000f;
    public float wheelieAngle = 45f;
    public float minAngle = 45f;
    public float maxAngle = 90f;

    public WheelSettings wheelSettings = null;
}