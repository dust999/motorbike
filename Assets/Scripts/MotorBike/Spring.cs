using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Spring : MonoBehaviour
{
    [SerializeField] private Transform _startPos = null;
    [SerializeField] private Transform _endPos = null;

    [SerializeField] private LineRenderer _lineRender = null;

    private void Reset()
    {
        _lineRender = GetComponent<LineRenderer>();
        _lineRender.useWorldSpace = true;
    }

    private void OnEnable()
    {
        StartCoroutine(UpadteSpring());
    }

    private IEnumerator UpadteSpring()
    {
        Vector3 [] points = new Vector3 [2];
        var waitForFixedUpdate = new WaitForFixedUpdate();
        
        while (true)
        {
            points[0] = _startPos.position;
            points[1] = _endPos.position;
            
            _lineRender.SetPositions(points);
    
            yield return waitForFixedUpdate;
        }
    }


    private void OnDisable()
    {
        StopAllCoroutines();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_lineRender == null)
            _lineRender = GetComponent<LineRenderer>();
    }

    private void OnDrawGizmos()
    {
        Vector3[] points = new Vector3[2];

        if (_startPos == null || _endPos == null) return;

        points[0] = _startPos.position;
        points[1] = _endPos.position;

        _lineRender.SetPositions(points);
    }
#endif
}
