using UnityEngine;

public class DistanceMeter : MonoBehaviour
{
    private Vector3 _startPosition;
    public float distance { get; private set; } = 0;

    private void Start()
    {
        _startPosition = transform.position;
    }
    private void Update()
    {
        distance = transform.position.x - _startPosition.x;
        if (distance < 0) distance = 0;

        distance = Mathf.Round(distance);
    }
}