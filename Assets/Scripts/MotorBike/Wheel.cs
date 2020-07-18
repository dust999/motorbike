using UnityEngine;

public class Wheel : MonoBehaviour
{
    private WheelSettings _wheelSettings;
    private Rigidbody2D _rigidbody2D = null;
    private WheelJoint2D _wheelJoint = null;
    private GetOnRoad _getOnRoad = null;
    [SerializeField] private ParticleSystem _particles = null;
    private ParticleSystem.EmissionModule _emission;
    private JointMotor2D _motor;

    public void Setup(WheelJoint2D wheel, WheelSettings wheelSettings)
    {
        _wheelSettings = wheelSettings;

        _getOnRoad =  GetComponent<GetOnRoad>();
        _rigidbody2D =GetComponent<Rigidbody2D>();

        _wheelJoint = wheel;
        _motor.maxMotorTorque = wheelSettings.motorToque;
        SetMotorSpeed(0);

        _emission = _particles.emission;
        _particles.transform.parent = null;
    }

    private void SetMotorSpeed(float speed)
    {
        _motor.motorSpeed = speed * -1;
        _wheelJoint.motor = _motor;
    }

    private void UseMotor(bool isUsing = true)
    {
        _wheelJoint.useMotor = isUsing;
    }

    public void Accelerate(float speed)
    {
        if (    Mathf.Abs(_wheelJoint.motor.motorSpeed) > _wheelSettings.accelerateShowSmoke && 
                _rigidbody2D.velocity.x < _wheelSettings.smokeRigidBodyVelocity
            )
            ShowSmoke();

        SetMotorSpeed(speed);
        UseMotor();
    }

    public void Neutral()
    {
        UseMotor(false);
    }

    public void Brake()
    {
        if ( 
            Mathf.Abs(_rigidbody2D.velocity.x) > _wheelSettings.smokeRigidBodyVelocity &&
            _wheelJoint.motor.motorSpeed <= 0
            )
            ShowSmoke();

        SetMotorSpeed(0);
        UseMotor();
    }

    public float GetSpeed()
    {
        return _wheelJoint.motor.motorSpeed;
    }

    public bool IsOnRoad()
    {
        return _getOnRoad.isOnRoad;
    }

    public void Freeze(bool isFreezd = true)
    {
        if (isFreezd)
        {
            _rigidbody2D.angularVelocity = 0f;
            _rigidbody2D.velocity = Vector2.zero;
        }
         
        _rigidbody2D.freezeRotation = isFreezd;
        _rigidbody2D.isKinematic = isFreezd;
    }

    private void ShowSmoke(bool isShowing = true)
    {
        _emission.enabled = isShowing;

        if (!_getOnRoad.isOnRoad)
            _emission.enabled = false;
    }

    private void Update()
    {
        if (!IsOnRoad())
            ShowSmoke(false);
    }

    public void Reset()
    { 
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.rotation = 0f;
        
        Brake();
        UseMotor(true);
    }
}
