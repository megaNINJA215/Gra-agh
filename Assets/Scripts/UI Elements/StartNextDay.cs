using UnityEngine;

public class StartNextDay : MonoBehaviour {
    public void StartNextDayFn() {
        TimeManagement.Instance.StartTimeCounter();
    }
}
