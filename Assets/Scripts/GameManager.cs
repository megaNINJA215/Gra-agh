using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static bool isDayRunning;

    [Header("Human Queue")]
    [SerializeField] private HumanQueue humanQueue;
    [SerializeField] private Transform humanSpawnPosition;

    [Header("Player Stats")]
    public int Money = 5000;

    [Header("Game Components")]
    [SerializeField] private TextMeshProUGUI hajs;
    [SerializeField] private TextMeshProUGUI godzina;
    [SerializeField] private GameObject endOfDayUI;

    void Awake(){
        if(Instance == null){
            Instance = this;
        }
    }

    void Start() {
        TimeManagement.Instance.StartTimeCounter();

        hajs.SetText("Hajs: " + Money);
        isDayRunning = true;
    }

    private void OnEnable() {
        TimeManagement.on_time_progressed += UpdateHourCounter;
    }

    private void OnDisable() {
        TimeManagement.on_time_progressed -= UpdateHourCounter;
    }

    public void Buy(int value) {
        Money -= value;
        hajs.SetText("Hajs: " + Money);
    }

    public void AddMoney(int value) {
        Money += value;
        hajs.SetText("Hajs: " + Money);
    }

    private void UpdateHourCounter() {
        int start_hour = TimeManagement.Instance.startHour;
        int start_minute = TimeManagement.Instance.startMinute;
        int time = TimeManagement.Instance.time;

        godzina.SetText(TimeFormatter.Format(TimeManagement.current_day, start_hour, start_minute, time));
    }

    private IEnumerator CountTime() {
        while (time < 60 * avaibleHours) {
            if (time % 30 == 0) {
                humanQueue.SpawnRandomHuman(humanSpawnPosition.position); //TODO: umieścić to w sensownym miejscu, teraz dałem tutaj dla testu, respi się co 30 minut in game, można to również zrandomizować
            }

            godzina.SetText(TimeFormatter.Format(dayNumber, startHour, startMinute, time));
            yield return new WaitForSeconds(timeJump * timeScale);
            time += timeJump;
        }

        godzina.SetText(TimeFormatter.Format(dayNumber, startHour, startMinute, time));
        Time.timeScale = 0f;
        isDayRunning = false; // do blokowania inputu, nie miałem pomysłu jak zrobić inaczej
        endOfDayUI.SetActive(true);

        if (countTimeCoroutine != null) {
            StopCoroutine(countTimeCoroutine);
            countTimeCoroutine = null;
        }
    }
}