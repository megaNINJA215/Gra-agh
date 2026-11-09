using UnityEngine;

public class HumanQueueView : MonoBehaviour {
    [Header("Queue")]
    [SerializeField] private HumanQueue humanQueue;

    [Header("Positions")]
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Transform firstHumanPosition;
    [SerializeField] private Transform spacing;
    [SerializeField] private bool isBig;

    TimeManagement.RandomizedAction new_action;

    private void Start() {
        new_action = new(1, 4);
        new_action.randomizedEvent.AddListener(() => SpawnRandomHuman());
        new_action.randomizedEvent.AddListener(() => new_action.PickRandomizedTime());
        TimeManagement.Instance.randomized_actions.Add(new_action);

        humanQueue.StartNewScene(firstHumanPosition, spacing, isBig);
    }

    private void OnOestroy() {
        TimeManagement.Instance.randomized_actions.Remove(new_action);
    }

    public void SpawnRandomHuman() {
        if (humanQueue.humans.Count >= 10) {
            // Debug.Log("Osiągniętą maksymalną długość kolejki");
            return;
        }
        Debug.Log("resp");

        humanQueue.SpawnRandomHuman(spawnPosition.position, isBig);
        UpdateQueue();
    }

    public void RemoveFirstHuman() {
        if (humanQueue.RemoveFirstHuman()) {
            UpdateQueue();
        }
    }
    
    private void UpdateQueue() {
        int index = 0;

        foreach (HumanQueue.DataAndObject human in humanQueue.Humans) {
            Vector3 targetPosition = firstHumanPosition.position + spacing.position * index;

            human._gameobject.MoveTo(targetPosition);

            index++;
        }
    }
    
}
