using UnityEngine;

public class HumanQueueView : MonoBehaviour {
    [Header("Queue")]
    [SerializeField] private HumanQueue humanQueue;

    [Header("Positions")]
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Transform firstHumanPosition;
    [SerializeField] private float spacing;

    private void Start(){
        TimeManagement.RandomizedAction new_action = new(1, 4);
        new_action.randomizedEvent.AddListener(SpawnRandomHuman);
        new_action.randomizedEvent.AddListener(() => new_action.PickRandomizedTime());
    }

    public void SpawnRandomHuman() {
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
            Vector3 targetPosition = firstHumanPosition.position + new Vector3(-index * spacing, 0f, 0f);

            human.MoveTo(targetPosition);

            index++;
        }
    }
    
}
