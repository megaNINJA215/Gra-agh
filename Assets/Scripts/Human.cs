using UnityEngine;
using System.Collections;
using TMPro;

public class Human : MonoBehaviour
{
    private HumanData humanData;
    private SpriteRenderer spriteRenderer;
    private Coroutine movementCoroutine;
    private Vector3 currentTarget;

    private void Awake() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Initialize(HumanData data) {
        humanData = data;
        spriteRenderer.color = data._color; //TODO: zastąpić faktycznymi teksturami

        Debug.Log($"Spawned: {JsonUtility.ToJson(humanData, true)}");
    }

    private IEnumerator MoveCoroutine(Vector3 targetPosition) {
        float duration = Vector2.Distance(transform.position, targetPosition) / humanData.movement_speed;

        while (duration > 0f) {
            Debug.Log("Duration" + duration);

            duration = Vector2.Distance(transform.position, targetPosition) / humanData.movement_speed;

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, humanData.movement_speed * Time.deltaTime);

            yield return null;
        }

        transform.position = targetPosition;
        movementCoroutine = null;
    }

    public void MoveTo(Vector3 targetPosition)
    {
        if (movementCoroutine != null && currentTarget == targetPosition) {
            return;
        }

        if (movementCoroutine != null) {
            StopCoroutine(movementCoroutine);
            movementCoroutine = null;
        }

        currentTarget = targetPosition;

        movementCoroutine = StartCoroutine(
            MoveCoroutine(targetPosition)
        );
    }
}