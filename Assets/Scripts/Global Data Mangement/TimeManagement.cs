using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class TimeManagement : MonoBehaviour
{
    public static TimeManagement Instance { get; private set; }

    [Header("Game Time")]

    [field: SerializeField] public int time { get; private set; } = 0;
    [field: SerializeField] public int availableHours { get; private set; } = 8;
    [field: SerializeField] public int startHour { get; private set; } = 12;
    [field: SerializeField] public int startMinute { get; private set; } = 0;
    [field: SerializeField] public float timeScale { get; private set; } = 1f; // 1s irl -> timeScale min in game  

    private const int timeJump = 10; // co ile pokazuje zmiane godziny

    public static int current_day { get; private set; }

    public readonly List<RandomizedAction> randomized_actions = new();

    public static Action on_time_progressed;
    public static Action on_day_ended;

    Coroutine time_counting_coroutine;

    private void Awake() {
        if(Instance == null)
            Instance = this;
    }

    // nie wiedziałem gdzie to dać, we to gdzieś przenieś gdzie to będzie miało sens
    public void StartTimeCounter() {
        current_day += 1;
        time = 0;
        Time.timeScale = 1f;

        if (time_counting_coroutine != null)
            StopCoroutine(time_counting_coroutine);

        time_counting_coroutine = StartCoroutine(TimePassage());
        GameManager.isDayRunning = true;
    }

    public IEnumerator TimePassage() {
        while (time < 60 * availableHours) {
            foreach (RandomizedAction action in randomized_actions) {
                if (time % action.chosen_randomized_duration == 0) {
                    action.randomizedEvent?.Invoke();
                }
            }

            on_time_progressed?.Invoke();

            yield return new WaitForSeconds(timeJump * timeScale);

            time += timeJump;
        }

        Time.timeScale = 0f;
        GameManager.isDayRunning = false;

        on_day_ended?.Invoke();
    }

    public static void SetCurrentDay(int day){
        current_day = day;
    }

    [System.Serializable]
    public class RandomizedAction {
        /// <summary>
        /// Has to be between 0 and 6
        /// </summary>
        public int random_action_min = 1;
        /// <summary>
        /// Has to be between 0 and 6
        /// </summary>
        public int random_action_max = 4;

        [HideInInspector] public int chosen_randomized_duration;

        public int PickRandomizedTime(){
            chosen_randomized_duration = Random.Range(random_action_min, random_action_max) * timeJump;
            return chosen_randomized_duration;
        }

        public RandomizedAction(int random_action_min, int random_action_max) {
            this.random_action_min = Mathf.Clamp(random_action_min, 0, 6);
            this.random_action_max = Mathf.Clamp(random_action_max, 0, 6);
            chosen_randomized_duration = Random.Range(random_action_min, random_action_max) * timeJump;
        }

        public UnityEvent randomizedEvent;
    }
}

public static class TimeFormatter {
    public static string Format(int dayNumber, int startHour, int startMinute, int timeCounter) {
        int currentTime = startHour * 60 + startMinute + timeCounter;
        int hour = currentTime / 60 % 24;
        int minute = currentTime % 60;

        return string.Format("Dzień: {0}\nGodzina: {1}:{2}", dayNumber, hour < 10 ? "0" + hour : hour, minute < 10 ? "0" + minute : minute);
    }
}