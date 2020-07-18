using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Motorbike Settings")]
    [SerializeField] private MotorBike _motorBike = null;
    [SerializeField] private WheelieTitle _wheelie = null;
    [SerializeField] private FallOver _gameOver = null;

    [Header("Terrain Settings")]
    [SerializeField] private Terrain2DPooler _terrain2DPooler = null;
    [SerializeField] private Terrain2DSettings _terrain2DSettings = null;
    [SerializeField] private int terrainsCount = 2;

    [Header("UI")]
    [SerializeField] private Transform _camera = null;
    [SerializeField] private DistanceMeter _distanceMetr = null;
    private UpdateDistance _updateDistance = null;

    private Vector3 bikePosition;
    private Vector3 bikeRotation;

    private void Awake()
    {
        _updateDistance = GetComponent<UpdateDistance>();
        _updateDistance.Setup(_distanceMetr);

        _terrain2DPooler.Init(terrainsCount, _terrain2DSettings, _camera);
    }

    public void Start()
    {
        bikePosition = _motorBike.transform.position;
        bikeRotation = _motorBike.transform.rotation.eulerAngles;
        
        _wheelie.ShowHideWheelie(false);

        _motorBike.OnWheelie += _wheelie.ShowHideWheelie;
        _gameOver.fallOver += Reset;
    }

    public void Reset()
    {
        _motorBike.Reset();

        _motorBike.transform.position = bikePosition;

        Quaternion rotation = new Quaternion();
        rotation.eulerAngles = bikeRotation;
        _motorBike.transform.rotation = rotation;

        _terrain2DPooler.ResetTerrains();
    }
}
