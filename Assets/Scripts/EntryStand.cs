using UnityEngine;
using UnityEngine.InputSystem;

public class EntryStand : MonoBehaviour
{

    public void OnMouseOver() {
        if (Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame && GameManager.Instance.isDayRunning) {
            EnterEntryStand();
        }
    }

    public void EnterEntryStand() {
        Debug.Log("zmiana sceny");
    }
}
