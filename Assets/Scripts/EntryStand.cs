using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class EntryStand : MonoBehaviour
{
    [SerializeField] private Collider2D my_collider;

    public void EnterEntryStand() {
        if (GameManager.isDayRunning)
            return;

        UnityEngine.SceneManagement.SceneManager.LoadScene("EntryStand");
    }
}