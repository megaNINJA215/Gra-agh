using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class EntryStand : MonoBehaviour
{
    [SerializeField] private HumanQueue humanQueue;
    [SerializeField] private Collider2D my_collider;

    public void EnterEntryStand() {
        if (GameManager.Instance.isDayRunning && !humanQueue.RemoveFirstHuman()) {
            Debug.Log("Nikogo nie ma w kolejce");
        }
    }
}