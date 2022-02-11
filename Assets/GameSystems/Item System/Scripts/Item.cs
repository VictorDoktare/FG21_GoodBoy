using UnityEngine;

public class Item : MonoBehaviour
{
    public BaseItem item;
    [SerializeField] [Range(0, 100)] private int _rotationSpeed;
    private bool isDroped;
    public bool IsDroped { get => isDroped; set => isDroped = value; }

    private void Update()
    {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isDroped = false;
        }
    }
}