using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GetOnRoad : MonoBehaviour
{
    public bool isOnRoad { get; private set; } = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOnRoad = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isOnRoad = false;
    }
}
