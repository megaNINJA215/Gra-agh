using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isDayRunning;


    [Header("Human Queue")]
    [SerializeField] private HumanQueue humanQueue;
    [SerializeField] private Transform humanSpawnPosition;


    [Header("Player Stats")]
    public int Money = 5000;


    [Header("Game Time")]
    [SerializeField] private int time = 0;
	[SerializeField] private int avaibleHours = 8;
    [SerializeField] private int startHour = 12;
    [SerializeField] private int startMinute = 0;
    [SerializeField] private float timeScale = 1f; // 1s irl -> timeScale min in game  
    [SerializeField] private int timeJump = 10; // co ile pokazuje zmiane godziny
    [SerializeField] private int dayNumber = 0;
    [System.NonSerialized] public Coroutine countTimeCoroutine;


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
        hajs.SetText("Hajs: " + Money);
        StartTimeCounter();
        isDayRunning = true;
    }

    public void Buy(int value) {
        Money -= value;
        hajs.SetText("Hajs: " + Money);
    }

    public void AddMoney(int value) {
        Money += value;
        hajs.SetText("Hajs: " + Money);
    }

    // nie wiedziałem gdzie to dać, we to gdzieś przenieś gdzie to będzie miało sens
    public void StartTimeCounter() {
        dayNumber += 1;
        time = 0;
        Time.timeScale = 1f;
        countTimeCoroutine = StartCoroutine(CountTime());
        isDayRunning = true;
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


public static class TimeFormatter {
    public static string Format(int dayNumber, int startHour, int startMinute, int timeCounter) {
        int currentTime = startHour * 60 + startMinute + timeCounter;
        int hour = (currentTime / 60) % 24;
        int minute = currentTime % 60; 

        return string.Format("Dzień: {0}\nGodzina: {1}:{2}", dayNumber, hour < 10 ? "0" + hour : hour, minute < 10 ? "0" + minute : minute);
    }
}