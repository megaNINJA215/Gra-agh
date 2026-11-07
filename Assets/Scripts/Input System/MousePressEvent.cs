using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MousePressEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent LeftClickPressed;
    [SerializeField] private UnityEvent RightClickPressed;
    [SerializeField] private Collider2D my_collider;

    private void OnEnable() {
        UserInput.Instance.MouseClick.performed += PressedLeftMouseButton;
        UserInput.Instance.RightMouseClick.performed += PressedRightMouseButton;
    }

    private void OnDisable() {
        UserInput.Instance.MouseClick.performed -= PressedLeftMouseButton;
        UserInput.Instance.RightMouseClick.performed += PressedRightMouseButton;
    }

    private void PressedLeftMouseButton(InputAction.CallbackContext context) {
        Vector2 mouse_position = UserInput.Instance.MousePosition.ReadValue<Vector2>();
        Vector2 position_to_check = Camera.main.ScreenToWorldPoint(mouse_position);

        if (my_collider.bounds.Contains(position_to_check)) {
            LeftClickPressed?.Invoke();
        }
    }

    private void PressedRightMouseButton(InputAction.CallbackContext context) {
        Vector2 mouse_position = UserInput.Instance.MousePosition.ReadValue<Vector2>();
        Vector2 position_to_check = Camera.main.ScreenToWorldPoint(mouse_position);

        if (my_collider.bounds.Contains(position_to_check)) {
            RightClickPressed?.Invoke();
        }
    }
}