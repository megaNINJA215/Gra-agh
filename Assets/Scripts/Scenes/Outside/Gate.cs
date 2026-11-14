using UnityEngine;

public class Gate : MonoBehaviour {

    private bool usedToday = false;
    
    private void OnEnable() {
        TimeManagement.on_day_started += ResetGateUsage;
    }

    private void OnDisable() {
        TimeManagement.on_day_started -= ResetGateUsage;
    }

    public void EnterGate() {
        if (!GameData.isDayRunning)
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

    private void ResetGateUsage() {
        usedToday = false;
    }
}
