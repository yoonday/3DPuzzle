using UnityEngine;

public class DoorObject : MonoBehaviour, IButtonLinkedObject
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnButton()
    {
        animator.SetBool("Button", true);
    }

    public void EndButton()
    {
        animator.SetBool("Button", false);
    }
}