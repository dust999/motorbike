using UnityEngine;

public class Terrain2DPooler : MonoBehaviour
{
    private Terrain2D[] _terrains2D = null;
    
    private Transform _followTarget = null;

    private float _terrain2DLenght = 0f;
    private int _currentPice = 0;
    private int _lastPice = 0;

    public void Init( int terrainsCount , Terrain2DSettings settings , Transform followTarget )
    {
        if (terrainsCount < 2) terrainsCount = 2;

        _terrains2D = new Terrain2D [terrainsCount];

        for (int i = 0; i < terrainsCount; i++)
        {
            GameObject terrain = new GameObject("Terrain");
            terrain.transform.parent = transform;

            _terrain2DLenght = settings.meshOffset * (settings.meshResolution - 1) * 2f;

            terrain.transform.localPosition = Vector3.right * _terrain2DLenght * i;

            _terrains2D[i] = new Terrain2D(terrain, settings);
        }

        _currentPice = 0;
        _followTarget = followTarget;
    }
   
    private void Update()
    {
        if (_followTarget == null) return;

        float switchPosition = _followTarget.transform.position.x + (_terrain2DLenght - transform.position.x) * 0.5f;
        _currentPice = (int)(switchPosition / _terrain2DLenght);

        if (_currentPice == _lastPice) return;      

        ActivateTerrain(_currentPice - 2, false); // DEACTIVATE LAST
        ActivateTerrain(_currentPice, true); // ACTIVATE NEXT

        _lastPice = _currentPice;
    }

    private int GetTerrainID(int id)
    {
        id = id % _terrains2D.Length;
        if (id >= 0) return id;
        return  _terrains2D.Length + id;
    }

    private void ActivateTerrain(int id, bool isActive, float offset = 0)
    {
        if (isActive && _terrains2D[GetTerrainID(id)] != null)
            _terrains2D[GetTerrainID(id)].terrainGO.transform.localPosition = Vector3.right * _terrain2DLenght * id;

        _terrains2D[GetTerrainID(id)]?.UpdateTerrain();
        _terrains2D[GetTerrainID(id)]?.terrainGO.SetActive(isActive);
    }

    public void ResetTerrains()
    {
        _currentPice = 0;
        _lastPice = 0;

        for (int i = 0; i < _terrains2D.Length; i++)
            _terrains2D[i]?.UpdateTerrain();

        ActivateTerrain(_currentPice, true); // ACTIVATE NEXT
    }
}