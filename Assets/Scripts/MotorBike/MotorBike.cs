using UnityEngine;

[RequireComponent(typeof(WheelJoint2D))]
public class MotorBike : MonoBehaviour
{
    [SerializeField] MotorBikeSettings _settings = null;
    [SerializeField] private WheelJoint2D _rearJoint2D = null;
    [SerializeField] private WheelJoint2D _frontJoint2D = null;
    [SerializeField] private Wheel _rearWheel = null;
    [SerializeField] private Wheel _frontWheel = null;

    private Rigidbody2D _rigidbody2D = null;

    public delegate void IsWheelie(bool isActive);
    public event IsWheelie OnWheelie;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        SetupMotorBike();
    }
    private void SetupMotorBike()
    {
        _rearWheel.Setup (_rearJoint2D, _settings.wheelSettings);
        _frontWheel.Setup(_frontJoint2D, _settings.wheelSettings);
    }

    private void CheckWheelie()
    {
        if ( _rigidbody2D.rotation >= _settings.minAngle    &&
            _rigidbody2D.rotation < _settings.maxAngle      &&
            !_frontWheel.IsOnRoad()
            )
        {
            OnWheelie?.Invoke(true);
        }
        else
        {
            OnWheelie?.Invoke(false);
        }
    }

    public void Accelerate()
    {
        _rearWheel.Accelerate(_settings.maxSpeed);
        _frontWheel.Neutral();
    }

    public void Break()
    {
        _rearWheel.Brake();
        _frontWheel.Brake();
    }

    public void Neutral()
    {
        _rearWheel.Neutral();
        _frontWheel.Neutral();
    }

    private void FixedUpdate()
    {
        CheckWheelie();
    }

    public void Reset()
    {
        _frontWheel.Reset();
        _rearWheel.Reset();

        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.angularVelocity = 0f;
    }
}
