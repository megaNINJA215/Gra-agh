using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class UserInput : MonoBehaviour
{
    public static UserInput Instance { get; private set; }

    public static PlayerInput _player_input { get; private set; }
    public static System.Action on_controls_changed;

    //PLAYER
    public InputAction Movement { get; private set; }
    public InputAction MouseClick { get; private set; }
    public InputAction RightMouseClick { get; private set; }
    public InputAction MousePosition { get; private set; }

    //UI
    public InputAction Navigate { get; private set; }
    public InputAction Submit { get; private set; }
    public InputAction Cancel { get; private set; }
    public InputAction Point { get; private set; }
    public InputAction Click { get; private set; }
    public InputAction ScrollWheel { get; private set; }
    public InputAction MiddleClick { get; private set; }
    public InputAction RightClick { get; private set; }

    private string current_control_sheme;

    public readonly string gameplay_action_map = "GamePlayMap";
    public readonly string UI_action_map = "UI";

    private void Awake() {
        Instance = this;

        _player_input = GetComponent<PlayerInput>();

        Rebind();

        //if (_player_input.currentControlScheme == null)
        //    _player_input.SwitchCurrentControlScheme(_player_input.defaultControlScheme);

        //current_control_sheme = _player_input.currentControlScheme;

        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change) {}

    private void Rebind() {

        Movement = _player_input.actions["Movement"];
        MouseClick = _player_input.actions["MouseClick"];
        RightMouseClick = _player_input.actions["RightMouseClick"];
        MousePosition = _player_input.actions["MousePosition"];

        Navigate = _player_input.actions["Navigate"];
        Submit = _player_input.actions["Submit"];
        Cancel = _player_input.actions["Cancel"];
        Point = _player_input.actions["Point"];
        Click = _player_input.actions["Click"];
        ScrollWheel = _player_input.actions["ScrollWheel"];
        MiddleClick = _player_input.actions["MiddleClick"];
        RightClick = _player_input.actions["RightClick"];

        _player_input.ActivateInput();

        Debug.Log("Rebinded");
    }

    private void OnEnable() {
        if (Instance != this) {
            Instance = this;
            Rebind();
        }
    }

 //   private void Update() {
        //foreach (var device in InputSystem.devices){
        //    Debug.Log("---------------------------------------------------------------------");
        //    Debug.Log(device.device);
        //    Debug.Log(device.name);
        //}

        //if (UsesKeyboard())
        //    _player_input.uiInputModule.deselectOnBackgroundClick = true;
        //else if (UsesGamepad())
        //    _player_input.uiInputModule.deselectOnBackgroundClick = false;

        //if (!current_control_sheme.Equals(_player_input.currentControlScheme)) {
        //    current_control_sheme = _player_input.currentControlScheme;
        //    on_controls_changed?.Invoke();
        //}
 //   }

    public void ChangeCurrentActionMap(string action_map){
        _player_input.SwitchCurrentActionMap(action_map);
    }
}