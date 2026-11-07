using UnityEngine;

public class Tent : MonoBehaviour //, Interactable
{
    public TentData tentData;

    void Start() {
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = tentData._sprite;
    }

    public void UpgradeTent() {
        if (!GameData.isDayRunning)
            return;

        if (tentData._nextLevel == null) {
            Debug.Log("już jest maksymalny poziom");
            return;
        }

        if (!tentData.CanPurchase(GameData.money)) {
            Debug.Log("Nie stać cię");
            return;
        }

        GameManager.Instance.Buy(tentData._upgrade_price);
        tentData = tentData._nextLevel;

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = tentData._sprite;

        Debug.Log("Zakupiony poziom: "+ tentData._level);
    }
}

// public interface Interactable{
//     void Interact();
//     bool CanInteract();
// }