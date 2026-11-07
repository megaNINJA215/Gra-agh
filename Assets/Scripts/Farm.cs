using UnityEngine;
using UnityEngine.InputSystem;

public class Farm : MonoBehaviour
{

    public void OnMouseOver() {
        if (Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame && GameManager.Instance.isDayRunning) {
            ClickFarm();
        }
    }

    public void ClickFarm() {
        Debug.Log("Kliknięto farmę");
    }
}
