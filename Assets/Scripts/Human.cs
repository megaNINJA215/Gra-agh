using UnityEngine;
using System.Collections;

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


    private IEnumerator MoveCoroutine(Vector3 targetPosition, float duration) {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / duration);

            transform.position = Vector3.Lerp(
                startPosition,
                targetPosition,
                progress
            );

            yield return null;
        }

        transform.position = targetPosition;
        movementCoroutine = null;
    }

    public void MoveTo(Vector3 targetPosition, float duration)
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
            MoveCoroutine(targetPosition, duration)
        );
    }
}