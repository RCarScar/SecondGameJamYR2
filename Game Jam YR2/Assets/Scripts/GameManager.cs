using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static PlayerInput pInput;
    public static InputActions actions;

    private void Reset()
    {
        Instance = this; //setup static GM instance
    }

    void Awake()
    {
        // - setup input - //
        pInput = GetComponent<PlayerInput>();
        actions = new InputActions();
        actions.Enable(); 
    }

    // Update is called once per frame
    void Update()
    {

    }
}
