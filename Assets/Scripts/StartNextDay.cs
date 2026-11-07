using UnityEngine;

public class StartNextDay : MonoBehaviour
{
    [SerializeField] private GameObject endOfDayUI;
    [SerializeField] private GameObject gateObject;
    private Gate gate;

    void Start() {
        gate = gateObject.GetComponent<Gate>();
    }

    public void StartNextDayFn() {
        endOfDayUI.SetActive(false);
        gate.StartDay();
        TimeManagement.Instance.StartTimeCounter();
    }
}
