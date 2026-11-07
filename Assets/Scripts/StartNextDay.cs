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
        if (GameManager.Instance.countTimeCoroutine != null) {
            StopCoroutine(GameManager.Instance.countTimeCoroutine);
            GameManager.Instance.countTimeCoroutine = null;
        }
        endOfDayUI.SetActive(false);
        Time.timeScale = 1f;
        gate.StartDay();
        GameManager.Instance.StartTimeCounter();
    }
}
