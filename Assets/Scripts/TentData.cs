using UnityEngine;

[CreateAssetMenu(fileName = "TentData", menuName = "GameData/Interactables/New Tent Data")]
public class TentData : ScriptableObject {

  public int _upgrade_price;
  public int _level;
  public Sprite _sprite;
  public TentData _nextLevel;

  
  public bool CanPurchase(int money_to_check){
    return money_to_check >= _upgrade_price;
  }
}
