using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/TerrainSettings" , fileName = "Terrain2DSettings")]
public class Terrain2DSettings : ScriptableObject
{
    [Header("Mesh settings")]
    public int meshResolution = 64;
    public float meshOffset = 1f;
    public float topLine = 1f;
    public float bottomLine = -5f;
    [Range(0f, 0.5f)] public float smoothEdgesPercent = 0.1f;

    [Header("Noise and power of hills")]
    public float hillsMultiply = 3f;
    public float noiseFrequency = 10f;
    [Range(-0.5f, 0.5f)] public float noiseMiddlePoint = 0;

    [Header("Physics Settings")]
    public bool isPhysics = true;

    [Header("Render Settings")]
    public Material material = null;
    public int renderOrder = 0;
}
