using UnityEngine;

public class BulletFlight : MonoBehaviour
{
    [SerializeField] private float speed;
    public float damage;

    private void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime * speed);
    }
}
