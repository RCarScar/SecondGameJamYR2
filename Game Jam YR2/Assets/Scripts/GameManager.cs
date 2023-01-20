using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(ScreenShakeManager))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static PlayerInput pInput;
    public static InputActions Actions;
    public static ScreenShakeManager ScreenShake;

    private void Reset()
    {
        Instance = this; //setup static GM instance
        ScreenShake = GetComponent<ScreenShakeManager>();
    }

    void Awake()
    {
        // - setup input - //
        pInput = GetComponent<PlayerInput>();
        Actions = new InputActions();
        Actions.Enable(); 
    }

    // Update is called once per frame
    void Update()
    {

    }
}
