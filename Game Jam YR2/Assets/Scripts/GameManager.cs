using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(ScreenShakeManager))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static PlayerInput pInput;
    public static InputActions Actions;
    public static ScreenShakeManager ScreenShake;

    public Image Blinder;
    public float BlindSpeed = 5;
    public bool Blinded = false;

    [HideInInspector] public Volume postProcessing;
    private Vignette vignette;


    // --------------- Blinder Data ---------------- //

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
        postProcessing.profile.TryGet(out vignette);
    }

    // Update is called once per frame
    void Update()
    {
        if(Blinder)
        {
            // Change the alpha value of the blinder
            Blinder.color = new Color(Blinder.color.r, Blinder.color.g, Blinder.color.b,
                Mathf.MoveTowards(Blinder.color.a, Blinded ? 1 : 0, Time.deltaTime * BlindSpeed));
        }
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
