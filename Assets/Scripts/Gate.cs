using UnityEngine;
using UnityEngine.InputSystem;

public class Gate : MonoBehaviour {

    private bool usedToday = false;

    public void EnterGate() {
        if (!GameManager.Instance.isDayRunning)
            return;

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
