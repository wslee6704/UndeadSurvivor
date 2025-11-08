using UnityEngine;

public class AimToMouse : MonoBehaviour
{
    public RectTransform toggleUI;

    void Start()
    {
        toggleUI = this.GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        toggleUI.position = mousePos; // UI를 마우스 위치에 붙이기
    }
}
