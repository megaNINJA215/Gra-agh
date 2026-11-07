using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "HumanQueue", menuName = "GameData/Human Queue")]
public class HumanQueue : ScriptableObject {
    [SerializeField] private GameObject humanPrefab;
    private Queue<Human> humans = new Queue<Human>();
    private Vector3 spawnPosition = new Vector3(-10f, -1f, 0f);

    public void SpawnRandomHuman()
    {
        GameObject spawnedHuman = Instantiate(
            humanPrefab,
            spawnPosition,
            Quaternion.identity
        );

        HumanData humanData = CreateInstance<HumanData>();


        float[] professionChances = { 0.5f, 0.3f, 0.15f, 0.05f }; 
        float[] lootQualityChances = { 0.5f, 0.3f, 0.13f, 0.06f, 0.01f };

        
        humanData._name = "Human " + Random.Range(1, 100);

        humanData._profession = (Profession)WeightedRandom.GetRandom(professionChances);
        humanData._lootQuality = (LootQuality)WeightedRandom.GetRandom(lootQualityChances);

        //TODO: uzależnić te trzy od profesji
        humanData._minSurvivalDays = Random.Range(1, 5);
        humanData._maxSurvivalDays = Random.Range(humanData._minSurvivalDays+1, 15);
        humanData._survivalProbability = Random.Range(0.0f, 1.0f);

        humanData._color = Color.red;
        switch (humanData._profession) {
            case Profession.Knight:
                humanData._color = Color.gray;
                break;
            case Profession.Peasant:
                humanData._color = Color.green;
                break;
            case Profession.Trader:
                humanData._color = Color.orange;
                break;
            case Profession.Child:
                humanData._color = Color.cyan;
                break;
        }

        Human human = spawnedHuman.GetComponent<Human>();
        spawnedHuman.name = humanData._name;

        human.Initialize(humanData);
        humans.Enqueue(human);
        UpdateQueue(5f);
    }

    private void UpdateQueue(float duration) {
        int index = 0;

        foreach (Human human in humans) {
            Vector3 targetPosition = new Vector3(
                5f - index,
                -1f,
                0f
            );

            human.MoveTo(targetPosition, duration);

            index++;
        }
    }

    public bool RemoveFirstHuman() {
        while (humans.Count > 0)
        {
            Human firstHuman = humans.Dequeue();

            if (firstHuman == null)
                continue;

            Destroy(firstHuman.gameObject);

            //!! jeśli usuniesz kogoś z kolejki gdy ktoś dopiero do niej zmierza, 
            //!! to jego czas na dojście na pozycje zostaje zmieniony i znacząco przyśpiesza
            //!! ale póki co tak to zostawiam bo docelowo osoby z kolejki będzie się usuwać w innej scenie, gdzie nie bedzie to widoczne
            UpdateQueue(0.5f);
            return true;
        }

        return false;
    }
}


public static class WeightedRandom {
    public static int GetRandom(float[] chances) {

      float random = Random.Range(0f, 1f);
      for (int i = 0; i < chances.Length; i++) {
          random -= chances[i];
          if (random < 0f)
              return i;
      }
      return chances.Length - 1;
    }
}