using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class FallOver : MonoBehaviour
{
    public UnityAction fallOver = null;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        fallOver?.Invoke();
    }

    private void Update()
    {
        if (transform.position.y < -10)
            fallOver?.Invoke();
    }
}
