using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "HumanQueue", menuName = "GameData/Human Queue")]
public class HumanQueue : ScriptableObject {

  public Sprite knight;
  public Sprite child;
  public Sprite peasant;
  public Sprite trader;

    public class DataAndObject
    {
        public HumanData _data;
        public Human _gameobject;
        public DataAndObject(HumanData humanData, Human spawnedHuman)
        {
            this._data = humanData;
            this._gameobject = spawnedHuman;
        }
    }


    [SerializeField] private Human human_prefab;
    [SerializeField] private Human human_prefab_big;
    public Queue<DataAndObject> humans = new();
    public IEnumerable<DataAndObject> Humans => humans;

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

        switch (humanData._profession) {
            case Profession.Knight:
                humanData._sprite = knight;
                break;
            case Profession.Peasant:
                humanData._sprite = peasant;
                break;
            case Profession.Trader:
                humanData._sprite = trader;
                break;
            case Profession.Child:
                humanData._sprite = child;
                break;
        }

        spawnedHuman.name = humanData._name;

        spawnedHuman.Initialize(humanData);
        humans.Enqueue(new(humanData, spawnedHuman));
    }

    public bool RemoveFirstHuman() {
        while (humans.Count > 0) {
            DataAndObject firstHuman = humans.Dequeue();
            if (firstHuman == null)
                continue;

            Destroy(firstHuman._gameobject.gameObject);
            return true;
        }
        return false;
    }

    private Human GenerateObjectFromData(HumanData data, Transform firstHumanPosition, Transform spacing, bool isBig, int index) {
        Human spawnedHuman = Instantiate(
            isBig ? human_prefab_big : human_prefab,
            firstHumanPosition.position + spacing.position * index,
            Quaternion.identity
        );

        spawnedHuman.name = data._name;
        spawnedHuman.Initialize(data);

        return spawnedHuman;
    }

    public void StartNewScene(Transform firstHumanPosition, Transform spacing, bool isBig) {
        int index = 0;
        foreach (DataAndObject human in humans) {
            human._gameobject = GenerateObjectFromData(human._data, firstHumanPosition, spacing, isBig, index);
            index++;
        }
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