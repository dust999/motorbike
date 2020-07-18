using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WheelieTitle : MonoBehaviour
{
    private Animator _animator = null;
    [SerializeField] private string _showTrigger = "Show";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void ShowHideWheelie(bool isActive)
    {
        _animator.SetBool(_showTrigger, isActive);
    }
}
