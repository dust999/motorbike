using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/WheelSettings", fileName = "WheelSettings")]
public class WheelSettings : ScriptableObject
{
    public float motorToque = 1000f;

    [Header("Speed to show smoke")]
    public float breakShowSmoke = 5f;
    public float accelerateShowSmoke = 2000f;
    public float smokeRigidBodyVelocity = 5f;
}