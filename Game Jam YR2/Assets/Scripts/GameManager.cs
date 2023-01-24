/* Made by Blake Rubadue */

using Sounds;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(ScreenShakeManager))]
[RequireComponent(typeof(SoundPlayer))]
public class GameManager : MonoBehaviour
{
    // ----------- Static Variables ------------ //
    public static GameManager Instance;
    public static PlayerInput pInput;
    public static InputActions Actions;
    public static ScreenShakeManager ScreenShake;
    public static SoundPlayer AudioPlayer;

    // ----------- Events ----------- //
    public static UnityEvent PlayerDeathEvent = new UnityEvent();

    public static UnityEvent OpenEyesStartEvent = new UnityEvent();
    public static UnityEvent CloseEyesStartEvent = new UnityEvent();
    public static UnityEvent OpenEyesVisualEvent = new UnityEvent();
    public static UnityEvent CloseEyesVisualEvent = new UnityEvent();
    public static UnityEvent BlinkEvent = new UnityEvent();

    public Blinder Blinder;

    public SpriteRenderer GrowthBlinder;
    public SpriteRenderer FadeBlinder;
    public Vector3 BlindedScale = Vector3.zero;
    public float BlindSpeed = 10;

    public bool Blinded = false;

    // --------------- Blinder Data ---------------- //
    private Vector3 initBlindScale = Vector3.one;

    private void Reset()
    {
        Instance = this; //setup static GM instance

    }

    void Awake()
    {
        // - setup input - //
        pInput = GetComponent<PlayerInput>();
        Actions = new InputActions();
        Actions.Enable();

        ScreenShake = GetComponent<ScreenShakeManager>();
        AudioPlayer = GetComponent<SoundPlayer>();

        initBlindScale = GrowthBlinder.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

        /*// Change the scale of the growth blinder
        GrowthBlinder.transform.localScale = Vector3.MoveTowards(GrowthBlinder.transform.localScale,
                Blinded ? BlindedScale : initBlindScale, Time.deltaTime * BlindSpeed);

        // Change the alpha value of the fade blinder
        FadeBlinder.color = new Color(FadeBlinder.color.r, FadeBlinder.color.g, FadeBlinder.color.b,
            Mathf.MoveTowards(FadeBlinder.color.a, Blinded ? 1 : 0, Time.deltaTime * BlindSpeed));*/
    }

    public void Blind(InputAction.CallbackContext context)
    {
        if(context.started && !Blinded)
        {
            Blinder.CloseEyes();
            Blinded = true;
        }
        else if(context.started && Blinded)
        {
            Blinder.OpenEyes();
            Blinded = false;
        }
    }

    public void Blink(float Speed)
    {
        // Change the scale of the growth blinder
        GrowthBlinder.transform.localScale = Vector3.MoveTowards(GrowthBlinder.transform.localScale,
                Blinded ? BlindedScale : initBlindScale, Time.deltaTime * Speed);

        // Change the alpha value of the fade blinder
        FadeBlinder.color = new Color(FadeBlinder.color.r, FadeBlinder.color.g, FadeBlinder.color.b,
            Mathf.MoveTowards(FadeBlinder.color.a, Blinded ? 1 : 0, Time.deltaTime * Speed));
    }

    public void PlayerDeath()
    {

    }
}
