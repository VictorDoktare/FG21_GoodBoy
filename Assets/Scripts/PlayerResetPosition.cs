using UnityEngine;

public class PlayerResetPosition : MonoBehaviour
{
    private Vector3 playerPos;
    private Quaternion playerRot;
    private void Awake()
    {
        playerPos = gameObject.transform.position;
        playerRot = gameObject.transform.rotation;
    }

    public void ResetPlayerPos()
    {
        Debug.Log("Reseting player pos");
        transform.position = new Vector3(playerPos.x, playerPos.y, playerPos.z);
        transform.rotation = new Quaternion(playerRot.x, playerRot.y, playerRot.z, playerRot.w);
    }
}
