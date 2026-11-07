using UnityEngine;

public class StartNextDay : MonoBehaviour
{
    [SerializeField] private GameObject endOfDayUI;
    [SerializeField] private GameObject gateObject;
    private Gate gate;

    void Start() {
        if (gateObject != null) {
            gate = gateObject.GetComponent<Gate>();
        }
    }

    public void StartNextDayFn() {
        endOfDayUI.SetActive(false);
        if (gateObject != null) {
            gate.StartDay();
        }
        TimeManagement.Instance.StartTimeCounter();
    }
}
