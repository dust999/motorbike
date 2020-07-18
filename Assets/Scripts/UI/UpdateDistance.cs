using TMPro;
using UnityEngine;

public class UpdateDistance : MonoBehaviour
{
    [SerializeField] private TMP_Text text = null;
    private DistanceMeter _distanceMetr = null;
    private float _lastDistance = 0;

    public void Setup(DistanceMeter distanceMeter)
    {
        _distanceMetr = distanceMeter;
    }

    void Update()
    {
        if (_distanceMetr.distance == _lastDistance) return;
        text.text = _distanceMetr.distance.ToString();
    }
}
