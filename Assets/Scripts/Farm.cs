using UnityEngine;

public class Farm : MonoBehaviour
{
    public void ClickFarm() {
        if(GameData.isDayRunning)
            Debug.Log("Kliknięto farmę");
    }
}
