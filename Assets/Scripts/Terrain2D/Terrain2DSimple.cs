using UnityEngine;

public class Terrain2DSimple : MonoBehaviour
{
    [SerializeField] private Terrain2DSettings _settings = null;
    private Terrain2D _terrain2D = null;
    
    [SerializeField] private bool isDebugMode = false;
    public void Init ()
    {
        _terrain2D = new Terrain2D(gameObject, _settings);
    }

    private void Awake()
    {
        Init();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(isDebugMode)
            Init();
    }
#endif
}
