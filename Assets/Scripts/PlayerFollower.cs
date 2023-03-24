using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float smoothTime = 0.3f;

    private Vector3 offset;
    private Vector3 velocity = Vector3.zero;
    private void Start()
    {
        offset = transform.position - player.position;
    }
    private void LateUpdate()
    {
        Vector3 targetPosition = player.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
