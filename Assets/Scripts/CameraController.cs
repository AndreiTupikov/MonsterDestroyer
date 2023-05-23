using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerPosition;
    [SerializeField] private Transform leftBorder;
    [SerializeField] private Transform rightBorder;
    private bool pathIsOver;

    private void Update()
    {
        if (!pathIsOver && playerPosition != null && transform.position.x < playerPosition.position.x)
        {
            transform.position = new Vector3(playerPosition.position.x, transform.position.y, transform.position.z);
            if (Camera.main.ViewportToWorldPoint(new Vector2(1, 1)).x > rightBorder.position.x)
            {
                pathIsOver = true;
                return;
            }
            leftBorder.position = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        }
    }
}
