using UnityEngine;

public class ScoreboardPositionTracking : MonoBehaviour
{
    public Transform xrOrigin;
    public Transform playerCamera;
    public Vector3 offset = new Vector3(0f, 0.2f, 0.5f);
    public float followSpeed = 5f;


    // Update is called once per frame
    void Update()
    {
        Vector3 desiredPosition = playerCamera.position
                                  + playerCamera.forward * offset.z
                                  + playerCamera.right * offset.x
                                  + playerCamera.up * offset.y;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * followSpeed);

        Vector3 lookDirection = transform.position - playerCamera.position;
        lookDirection.y = 0; 
        transform.rotation = Quaternion.LookRotation(lookDirection.normalized);
    }
}
