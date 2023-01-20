using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(ScreenShakeManager))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static PlayerInput pInput;
    public static InputActions Actions;
    public static ScreenShakeManager ScreenShake;

    public bool Blinded = false;

    public float Val;

    [HideInInspector] public Volume postProcessing;
    private Vignette vignette;

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
        postProcessing = Camera.main.GetComponent<Volume>();
        postProcessing.profile.TryGet<Vignette>(out vignette);
    }

    // Update is called once per frame
    void Update()
    {
        //vignette.intensity = Val;
    }

    public void Blind(InputAction.CallbackContext context)
    {
        if(context.started && !Blinded)
        {
            Debug.Log("Blind");
            Blinded = true;
        }
        else if(context.started && Blinded)
        {
            Debug.Log("De-Blind");
            Blinded = false;
        }
        
    }
}
