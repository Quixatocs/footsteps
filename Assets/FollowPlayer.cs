using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerMovement.OnPlayerMoved += MoveToPlayer;
    }
    
    private void OnDisable()
    {
        PlayerMovement.OnPlayerMoved -= MoveToPlayer;
    }

    private void MoveToPlayer(Vector3 newPosition)
    {
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
    }
}
