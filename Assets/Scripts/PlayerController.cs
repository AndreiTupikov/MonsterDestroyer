using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Vector2 moveDirection;
    [SerializeField] private float speed;
    [SerializeField] private float health;
    public float reloadTime;
    [SerializeField] private HealthController healthBar;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject[] items;
    private Animator animator;
    private bool alive = true;
    private bool movingRight = true;
    [SerializeField] private Transform topBorder;
    [SerializeField] private Transform bottomBorder;
    [SerializeField] private Transform leftBorder;
    [SerializeField] private Transform rightBorder;

    private void Start()
    {
        healthBar.maxHealth = health;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (alive) MovePlayer();
    }

    private void MovePlayer()
    {
        if (moveDirection.x != 0 || moveDirection.y != 0)
        {
            if ((moveDirection.x < 0 && movingRight) || (moveDirection.x >= 0 && !movingRight)) ChangeDirection();
            animator.SetBool("isRunning", true);
            transform.Translate(moveDirection * Time.deltaTime * speed);
            MapBorderCorrection();
            healthBar.Move(transform.position, 1.3f);
        }
        else animator.SetBool("isRunning", false);
    }

    private void ChangeDirection()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        movingRight = !movingRight;
    }

    private void MapBorderCorrection()
    {
        if (transform.position.x > rightBorder.position.x) transform.position = new Vector2(rightBorder.position.x, transform.position.y);
        else if (transform.position.x < leftBorder.position.x) transform.position = new Vector2(leftBorder.position.x, transform.position.y);
        if (transform.position.y > topBorder.position.y) transform.position = new Vector2(transform.position.x, topBorder.position.y);
        else if (transform.position.y < bottomBorder.position.y) transform.position = new Vector2(transform.position.x, bottomBorder.position.y);
    }

    public void Shoot()
    {
        GameObject shot = Instantiate(bullet, transform.position + new Vector3(0, 0.35f, 0), Quaternion.Euler(0, movingRight ? 0 : 180, 0));
        Destroy(shot, 3);
    }

    public void Hit(float damage)
    {
        health -= damage;
        healthBar.Hit(damage);
        if (health <= 0)
        {
            alive = false;
            animator.SetTrigger("death");
            Destroy(healthBar.gameObject, 1.5f);
            Destroy(gameObject, 1.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.StartsWith("Loot"))
        {
            string itemName = collision.tag;
            if (items.Any(i => i.CompareTag(itemName)))
            {
                var item = items.SingleOrDefault(i => i.CompareTag(itemName)).transform.GetChild(0).gameObject.GetComponent<Text>();
                int count = int.Parse(item.text) + 1;
                item.text = count.ToString();
                item.gameObject.SetActive(true);
            }
            else
            {
                foreach(var item in items)
                {
                    if (item.CompareTag("Untagged"))
                    {
                        item.tag = collision.tag;
                        item.GetComponent<Image>().sprite = collision.GetComponent<SpriteRenderer>().sprite;
                        item.GetComponent<Button>().onClick.AddListener(() => { DeleteItem(item.tag); });
                        item.SetActive(true);
                        break;
                    }
                }
            }
            Destroy(collision.gameObject);
        }
    }

    public void DeleteItem(string tag)
    {
        foreach (var item in items)
        {
            if (item.CompareTag(tag))
            {
                var text = item.transform.GetChild(0).GetComponent<Text>();
                int count = int.Parse(text.text);
                if (count == 1)
                {
                    item.GetComponent<Button>().onClick.RemoveAllListeners();
                    item.GetComponent<Image>().sprite = null;
                    item.tag = "Untagged";
                    item.SetActive(false);
                }
                else
                {
                    text.text = (count - 1).ToString();
                    if (count == 2)
                    {
                        text.gameObject.SetActive(false);
                    }
                }
                break;
            }
        }
    }
}