using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "HumanQueue", menuName = "GameData/Human Queue")]
public class HumanQueue : ScriptableObject {
    [SerializeField] private Human human_prefab;
    private Queue<Human> humans = new Queue<Human>();
    public IEnumerable<Human> Humans => humans;

    public void SpawnRandomHuman(Vector2 position) {
        Human spawnedHuman = Instantiate(
            human_prefab,
            position,
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
        humanData._maxSurvivalDays = Random.Range(humanData._minSurvivalDays + 1, 15);
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

        spawnedHuman.name = humanData._name;

        spawnedHuman.Initialize(humanData);
        humans.Enqueue(spawnedHuman);
    }

    public bool RemoveFirstHuman() {
        while (humans.Count > 0) {
            Human firstHuman = humans.Dequeue();
            if (firstHuman == null)
                continue;

            Destroy(firstHuman.gameObject);
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