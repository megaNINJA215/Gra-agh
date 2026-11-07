using UnityEngine;
using UnityEngine.InputSystem;

public class Gate : MonoBehaviour {

    private bool usedToday = false;

    void OnMouseOver() {
        if (Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame && GameManager.Instance.isDayRunning) {
            EnterGate();
        }
    }

    public void EnterGate() {
        if (usedToday) {
            Debug.Log("Dzisiaj już byłeś w kryptcie");
            return;
        }
        int randomMoney = Random.Range(50,201);
        GameManager.Instance.AddMoney(randomMoney);
        Debug.Log("zarobiono:" + randomMoney);
        usedToday = true;
    }

    public void StartDay() {
        usedToday = false;
    }
}
