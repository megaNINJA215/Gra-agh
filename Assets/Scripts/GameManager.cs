using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    // public static bool isDayRunning;

    [Header("Human Queue")]
    [SerializeField] private HumanQueueView humanQueueView;

    [Header("Game Components")]
    [SerializeField] private TextMeshProUGUI hajs;
    [SerializeField] private TextMeshProUGUI godzina;
    [SerializeField] private GameObject endOfDayUI;

    void Awake(){
        if(Instance == null){
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
    }

    void Start() {
        hajs.SetText("Hajs: " + GameData.money);
        UpdateHourCounter();
        GameData.isDayRunning = true;
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
        GameData.money -= value;
        hajs.SetText("Hajs: " + GameData.money);
    }

    public void AddMoney(int value) {
        GameData.money += value;
        hajs.SetText("Hajs: " + GameData.money);
    }

    private void UpdateHourCounter() {
        int start_hour = GameData.startHour;
        int start_minute = GameData.startMinute;
        int time = GameData.time;

        if (godzina != null) {
            godzina.SetText(TimeFormatter.Format(GameData.currentDay, start_hour, start_minute, time));
        }
    }
}