using UnityEngine;

public class Terrain2D
{
    private MeshFilter _meshFilter = null;
    private MeshRenderer _meshRenderer = null;
    private Mesh _mesh = null;
    private PolygonCollider2D _polygonCollider = null;

    [SerializeField] private Terrain2DSettings _settings;

    private Vector3[]   _vertices;
    private int[]       _triangles;
    private Vector2[]   _uv;
    private Vector2[]   _polygonColliderVerices;

    public GameObject terrainGO { get; private set; } = null;

    public Terrain2D (GameObject gameObject, Terrain2DSettings settings)
    {
        if (gameObject == null || settings == null) return;

        terrainGO = gameObject;
        _settings = settings;

        int verticesCount = settings.meshResolution * 2; // TOP + BOTTOM
        int trianglesCount = verticesCount * 3;

        _vertices = new Vector3[verticesCount];
        _triangles = new int[trianglesCount];
        _uv = new Vector2[verticesCount];
        _polygonColliderVerices = new Vector2[settings.meshResolution + 2]; // TOP LINE + 2 BOTTOM POINTS FOR COLLIDER LEFT AND RIGHT

        _mesh = new Mesh();

        if (_meshRenderer == null) _meshRenderer = terrainGO.GetComponent<MeshRenderer>();
        if (_meshRenderer == null) _meshRenderer = terrainGO.AddComponent<MeshRenderer>();
        _meshRenderer.sortingOrder = settings.renderOrder;
        _meshRenderer.material = settings.material;

        if (_meshFilter == null) _meshFilter = terrainGO.GetComponent<MeshFilter>();
        if (_meshFilter == null) _meshFilter = terrainGO.AddComponent<MeshFilter>();
        _meshFilter.mesh = _mesh;

        if (_settings.isPhysics) {  // FAST ITTERATION NOT GOOD FOR PRODUCTION
            if (_polygonCollider == null) _polygonCollider = terrainGO.GetComponent<PolygonCollider2D>();
            if (_polygonCollider == null) _polygonCollider = terrainGO.AddComponent<PolygonCollider2D>();
        }

        UpdateTerrain();
    }
    private void GenerateTerrain()
    {
        int trianglesIndex = 0;
        float lastUVPoint = 0;
        float _randomForNoise = Random.Range(0, 10f); // USED FOR PERLIN NOISE

        for (int i = 0; i < _vertices.Length; i++)
        {
            Vector3 position = ( i % 2 == 0 ) ? TopLineGenerator( i, _randomForNoise ) : Vector3.up * _settings.bottomLine; //  TOP OR BOTTOM POSITION
            position.x += _settings.meshOffset * i - _settings.meshOffset * ( i % 2 ); // X OFFSET
            _vertices[i] = position;

            GenerateUV(ref lastUVPoint, i);
            
            if ( i > 1 && ( (i + 1 ) % 2) == 0 ) // PASS EACH 2 VERTICES START FROM 4
                GenerateTraingles(ref trianglesIndex, i + 1 ); // GENERATE 2 TRAINGLES OR ONE QUAD

            if( i % 2 == 0 && _settings.isPhysics) // PASS EACH TOP VERTICES FOR POLYGON COLLIDER
                AddPolygonColliderVerices(i);
        }

        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        _mesh.uv = _uv;

        _mesh.RecalculateBounds();
    }

    private Vector3 TopLineGenerator(int position, float _randomX)
    {
        float noise = Mathf.PerlinNoise(_randomX, position * 0.5f / _settings.meshResolution * _settings.noiseFrequency);
        noise -= 0.5f + _settings.noiseMiddlePoint;
        noise = SmoothOnEdges(noise * _settings.hillsMultiply, position, _settings.smoothEdgesPercent);

        Vector3 topOffset = Vector3.zero;
        topOffset.y += _settings.topLine;
        topOffset.y += noise;
        
        return Vector3.zero + topOffset;
    }

    private float SmoothOnEdges(float noise, int position, float percentOfLenght) // USED FOR SEAMLESS CONNECT TERRAINS
    {
        if (percentOfLenght == 0) return noise;
        if (percentOfLenght >= 0.5f) return 0f;

        position = position / 2; // RECALC TO ONLY TOP VERTICES

        int totalPoints = _settings.meshResolution-1;
        int smoothPoints = (int) (_settings.meshResolution * percentOfLenght);
        int rightEdge = totalPoints - smoothPoints;

        if (position == 0 || position == totalPoints)
        {
            return 0f; // Straight one level
        }

        if (position > smoothPoints && position <= totalPoints - smoothPoints)  // CENTERT
            return noise;
        else if (position < smoothPoints)                                       // LEFT EDGE
            return Mathf.SmoothStep(0, noise, position * 1f / smoothPoints * 1f);
        else                                                                    // RIGHT EDGE
            return Mathf.SmoothStep(noise, 0, (position-rightEdge) * 1f / smoothPoints * 1f);
    }

    private void GenerateTraingles(ref int tirs, int point)
    {
        int index = point - 4;

        _triangles[ tirs + 0 ] = index + 0;
        _triangles[ tirs + 1 ] = index + 2;
        _triangles[ tirs + 2 ] = index + 1;
        _triangles[ tirs + 3 ] = index + 1;
        _triangles[ tirs + 4 ] = index + 2;
        _triangles[ tirs + 5 ] = index + 3;

        tirs += 6;
    }
    private void GenerateUV(ref float lastUVpoint, int currentPoint)
    {
        float y = 1 - currentPoint % 2; // TOP - BOTTON
        float x = ( currentPoint % 2 == 0) ?  currentPoint * 1f / _settings.meshResolution * 1f : lastUVpoint;
       
        _uv[currentPoint] = new Vector2(x ,y);

        lastUVpoint = x;
    }

    private void AddPolygonColliderVerices(int i)
    {
        _polygonColliderVerices[ i / 2 ] = _vertices[i];
    }

    private void UpdatePolygonCollider()
    {
        _polygonColliderVerices[_polygonColliderVerices.Length - 2] = _vertices[_vertices.Length - 1]; // ADD LAST RIGHT BOTTOM VERTICE
        _polygonColliderVerices[_polygonColliderVerices.Length - 1] = _vertices[1]; // ADD FIRST LEFT BOTO VERTICE

        _polygonCollider.SetPath(0, _polygonColliderVerices); // UPDATE COLLIDER
    }

    private void DrawPoints() // USED FOR DEBUG
    {
        for(int i = 0; i < _vertices.Length; i++)
        {
            Gizmos.DrawSphere(_vertices[i] + terrainGO.transform.position, 0.5f) ;
        }
    }

    public void UpdateTerrain()
    {
        GenerateTerrain();
        if (_settings.isPhysics) UpdatePolygonCollider();  // FAST ITTERATION NOT GOOD FOR PRODUCTION
    }  
}
