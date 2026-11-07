using UnityEngine;

public class HumanData : ScriptableObject {

    public string _name;
    public Profession _profession;
    public LootQuality _lootQuality;
    public int _minSurvivalDays;
    public int _maxSurvivalDays;
    public float _survivalProbability; // 0.0f - 1.0f
    public Color _color;
}

public enum Profession {
    Knight, 
    Peasant,
    Trader,
    Child,
}

public enum LootQuality {
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}