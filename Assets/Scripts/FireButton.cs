using UnityEngine;

public class FireButton : MonoBehaviour
{
    public PlayerController playerController;
    private float lastShot;
    private float reload;

    private void Start()
    {
        reload = playerController.reloadTime;
    }

    private void Update()
    {
        if (lastShot + reload < Time.time)
        {
#if UNITY_EDITOR
            UseMouseInput();
#elif UNITY_ANDROID
        UseTouchScreenInput();
#endif
        }
    }

    private void UseTouchScreenInput()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    CheckHit(touch.position);
                }
            }
        }
    }

    private void UseMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckHit(Input.mousePosition);
        }
    }

    private void CheckHit(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.gameObject.name == "FireButton")
            {
                lastShot = Time.time;
                playerController.Shoot();
            }
        }
    }
}
