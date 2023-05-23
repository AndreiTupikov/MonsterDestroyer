using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public float maxHealth;
    private Image healthLevel;

    private void Start()
    {
        healthLevel = GetComponentInChildren<Image>();
    }

    public void Move(Vector2 position, float correction)
    {
        transform.position = new Vector2(position.x, position.y + correction);
    }

    public void Hit(float damage)
    {
        healthLevel.fillAmount -= damage / maxHealth;
        if (healthLevel.fillAmount < 0.3f) healthLevel.color = Color.red;
        else if (healthLevel.fillAmount < 0.6f) healthLevel.color = Color.yellow;
    }

}
