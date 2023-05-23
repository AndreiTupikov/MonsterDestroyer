using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private int enemiesCount;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private Transform canvas;
    [SerializeField] private Transform topBorder;
    [SerializeField] private Transform bottomBorder;
    [SerializeField] private Transform rightBorder;
    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private GameObject inventory;
    private SphereCollider[] uI;

    private void Start()
    {
        uI = canvas.GetComponentsInChildren<SphereCollider>();
        for(int i = 0; i < enemiesCount; i++)
        {
            MonsterSpawn();
        }
    }

    private void MonsterSpawn()
    {
        var monster = Instantiate(monsterPrefab, new Vector2(Random.Range(10f, rightBorder.position.x - 1), Random.Range(bottomBorder.position.y + 0.5f, topBorder.position.y - 0.5f)), Quaternion.identity);
        var healthBar = Instantiate(healthBarPrefab, new Vector2(monster.transform.position.x, monster.transform.position.y + 0.8f), Quaternion.identity, canvas);
        monster.GetComponent<FleshController>().Initialize(player.transform, healthBar);
    }

    public void InventoryOpen()
    {
        foreach (var element in uI) element.enabled = false;
        inventory.SetActive(true);
    }

    public void InventoryClose()
    {
        foreach (var element in uI) element.enabled = true;
        inventory.SetActive(false);
    }
}
