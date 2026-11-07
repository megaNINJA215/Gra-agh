using UnityEngine;

public class Farm : MonoBehaviour
{
    public void ClickFarm() {
        if(GameManager.Instance.isDayRunning)
            Debug.Log("Kliknięto farmę");
    }
}
