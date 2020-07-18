using UnityEngine;

public class Terrain2DPooler : MonoBehaviour
{
    private Terrain2D[] _terrains2D = null;
    
    private Transform _followTarget = null;

    private float _terrain2DLenght = 0f;
    private int _currenTerrain = 0;
    private int _lastTerrain = 0;

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

        _currenTerrain = 0;
        _followTarget = followTarget;
    }
   
    private void Update()
    {
        if (_followTarget == null) return;

        float targetXPos = _followTarget.transform.position.x;
        float hlafTerrain = _terrain2DLenght * 0.5f;
        float switchPosition = targetXPos  + hlafTerrain - transform.position.x;

        _currenTerrain = (int)(switchPosition / _terrain2DLenght);

        if (_currenTerrain == _lastTerrain) return;      

        ActivateTerrain(_currenTerrain - 2, false); // DEACTIVATE LAST
        ActivateTerrain(_currenTerrain, true); // ACTIVATE NEXT

        _lastTerrain = _currenTerrain;
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
            _terrains2D[GetTerrainID(id)].terrainGO.transform.localPosition = Vector3.right * _terrain2DLenght * id; //MOVE TERRAIN

        _terrains2D[GetTerrainID(id)]?.UpdateTerrain();
        _terrains2D[GetTerrainID(id)]?.terrainGO.SetActive(isActive);
    }

    public void ResetTerrains()
    {
        _currenTerrain = 0;
        _lastTerrain = 0;

        for (int i = 0; i < _terrains2D.Length; i++)
            _terrains2D[i]?.UpdateTerrain();

        ActivateTerrain(_currenTerrain, true); // ACTIVATE NEXT
    }
}