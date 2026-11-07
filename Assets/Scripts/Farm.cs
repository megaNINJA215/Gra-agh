using UnityEngine;

public class Farm : MonoBehaviour
{
    public void ClickFarm() {
        if(GameManager.Instance.gameData.isDayRunning)
            Debug.Log("Kliknięto farmę");
    }
}
