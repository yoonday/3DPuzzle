using UnityEngine;

public class LaserTrap : MonoBehaviour, IButtonLinkedObject
{
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private bool isWorking = true;
    private void Update()
    {
        if (isWorking)
        {
            Debug.DrawRay(transform.position, transform.forward, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, playerLayer))
            {
                Debug.Log("레이저 트랩 걸림");
            }
        }
        
    }

    public void OnButton()
    {
        isWorking = false;
    }

    public void EndButton()
    {

    }

    //private void Toggle()
    //{
    //    isWorking = !isWorking;
    //}
}