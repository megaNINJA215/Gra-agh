using UnityEngine;

public class BackButton : MonoBehaviour
{
    public void GoBack() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Outside");
    }
}
