using UnityEngine;

public class Farm : MonoBehaviour
{
    public void ClickFarm() {
        if(GameManager.isDayRunning)
            Debug.Log("Kliknięto farmę");
    }
}
