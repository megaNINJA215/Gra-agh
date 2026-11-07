using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static bool isDayRunning;

    [Header("Human Queue")]
    [SerializeField] private HumanQueueView humanQueueView;

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

    void ShowEndDayScreen() {
        endOfDayUI.SetActive(true);
    }

    private void OnEnable() {
        TimeManagement.on_time_progressed += UpdateHourCounter;
        TimeManagement.on_day_ended += ShowEndDayScreen;
    }

    private void OnDisable() {
        TimeManagement.on_time_progressed -= UpdateHourCounter;
        TimeManagement.on_day_ended -= ShowEndDayScreen;
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
}