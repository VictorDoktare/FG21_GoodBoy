using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    [Range(1f, 50f)] public float speed;

    [Range(75f, 300f)] public float rotationspeed;

    private void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            playerTransform.position += playerTransform.forward * (speed * Time.deltaTime);
        }
        
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            playerTransform.Rotate(Vector3.down * (rotationspeed * Time.deltaTime));
        }
        
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            playerTransform.position += playerTransform.forward * (-1 * speed * Time.deltaTime);
        }
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            playerTransform.Rotate(Vector3.up * (rotationspeed * Time.deltaTime));
        }
    }
}