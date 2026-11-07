using UnityEngine;
using UnityEngine.InputSystem;

public class EntryStand : MonoBehaviour
{
    [SerializeField] private HumanQueue humanQueue;

    public void OnMouseOver() {
        if (Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame && GameManager.Instance.isDayRunning) {
            EnterEntryStand();
        }
    }

    public void EnterEntryStand() {
        // if (!humanQueue.RemoveFirstHuman()) {
        //     Debug.Log("Nikogo nie ma w kolejce");
        // }
        UnityEngine.SceneManagement.SceneManager.LoadScene("EntryStand");
    }
}
