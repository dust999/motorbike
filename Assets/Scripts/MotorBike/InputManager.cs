using UnityEngine;

[RequireComponent(typeof(MotorBike))]
public class InputManager : MonoBehaviour
{
    private MotorBike _motorBike = null;

    private void Awake()
    {
        _motorBike = GetComponent<MotorBike>();
    }
    void Update()
    {
        if (!Input.GetMouseButton(0))
        {
            _motorBike.Neutral();
            return;
        }

        if(Input.mousePosition.x > Screen.width / 2)
        {
            _motorBike.Accelerate();
        }
        else
        {
            _motorBike.Break();
        }
    }
}
