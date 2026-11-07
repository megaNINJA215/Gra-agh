using UnityEngine;

public class HumanQueueView : MonoBehaviour {
    [Header("Queue")]
    [SerializeField] private HumanQueue humanQueue;

    [Header("Positions")]
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Transform firstHumanPosition;
    [SerializeField] private Transform spacing;

    private void Start() {
        TimeManagement.RandomizedAction new_action = new(1, 4);
        new_action.randomizedEvent.AddListener(() => SpawnRandomHuman());
        new_action.randomizedEvent.AddListener(() => new_action.PickRandomizedTime());
        TimeManagement.Instance.randomized_actions.Add(new_action);
    }

    public void SpawnRandomHuman() {
        if (humanQueue.humans.Count >= 3) {
            // Debug.Log("Osiągniętą maksymalną długość kolejki");
            return;
        }
        Debug.Log("resp");

        humanQueue.SpawnRandomHuman(spawnPosition.position);
        UpdateQueue();
    }

    public void RemoveFirstHuman() {
        if (humanQueue.RemoveFirstHuman()) {
            UpdateQueue();
        }
    }
    
    private void UpdateQueue() {
        int index = 0;

        foreach (Human human in humanQueue.Humans) {
            Vector3 targetPosition = firstHumanPosition.position + spacing.position * index;

            human.MoveTo(targetPosition);

            index++;
        }
    }
    
}
