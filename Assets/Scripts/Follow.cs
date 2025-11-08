using UnityEngine;

public class Follow : MonoBehaviour
{
    RectTransform rect;
    public float xDif = 0f;
    public float yDif = 0f;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void FixedUpdate()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);

        rect.position = new Vector3(screenPos.x + xDif, screenPos.y + yDif, screenPos.z);

    }
}
