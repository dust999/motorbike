using UnityEngine;

public class Terrain2DBackgorund : Terrain2DPooler
{
    [SerializeField] private Terrain2DSettings _settings = null;
#pragma warning disable 0649
    [SerializeField] private Transform _target;
    [SerializeField] private int _terrainsCount = 2;

    public void Start()
    {
        Init(_terrainsCount, _settings, _target);
    }
}