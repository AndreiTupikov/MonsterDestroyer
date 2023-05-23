using UnityEngine;

public class FleshController : MonoBehaviour
{
    [SerializeField] private float visionRange;
    [SerializeField] private float speed;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float damage;
    [SerializeField] private float health;
    [SerializeField] private GameObject[] loot;
    private bool isInitialized;
    private bool isTriggered;
    private bool movingLeft = true;
    private float lastAttack;
    private HealthController healthBar;
    private Transform target;
    private Animator animator;

    public void Initialize(Transform player, GameObject healthBar)
    {
        this.healthBar = healthBar.GetComponent<HealthController>();
        this.healthBar.maxHealth = health;
        target = player;
        animator = GetComponent<Animator>();
        isInitialized = true;
    }

    private void Update()
    {
        if (target!= null)
        {
            if (isInitialized && isTriggered)
            {
                if (Vector2.Distance(transform.position, target.position) < 1.2) Attack();
                else MoveToTarget();
            }
            else if (isInitialized)
            {
                if (Vector2.Distance(transform.position, target.position) < visionRange) isTriggered = true;
            }
        }
    }

    private void MoveToTarget()
    {
        animator.SetBool("isRunning", true);
        Vector2 direction = (target.position - transform.position).normalized;
        if ((direction.x <= 0 && !movingLeft) || (direction.x > 0 && movingLeft)) ChangeDirection();
        transform.Translate(direction * speed * Time.deltaTime);
        healthBar.Move(transform.position, 0.8f);
    }

    private void ChangeDirection()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        movingLeft = !movingLeft;
    }

    private void Attack()
    {
        animator.SetBool("isRunning", false);
        if (Time.time > lastAttack + (10 / attackSpeed))
        {
            animator.SetTrigger("attack");
            target.GetComponent<PlayerController>().Hit(damage);
            lastAttack = Time.time;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            health -= collision.GetComponent<BulletFlight>().damage;
            healthBar.Hit(collision.GetComponent<BulletFlight>().damage);
            Destroy(collision.gameObject);
            if (health <= 0) Defeat();
            if (!isTriggered) isTriggered = true;
        }
    }

    private void Defeat()
    {
        target = null;
        animator.SetTrigger("death");
        Instantiate(loot[Random.Range(0, loot.Length)], new Vector2(transform.position.x, transform.position.y - 0.5f), Quaternion.identity);
        Destroy(healthBar.gameObject, 1.5f);
        Destroy(gameObject, 1.5f);
    }
}