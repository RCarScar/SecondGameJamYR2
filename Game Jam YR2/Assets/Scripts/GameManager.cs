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
        ScreenShake = GetComponent<ScreenShakeManager>();
    }

    void Awake()
    {
        // - setup input - //
        pInput = GetComponent<PlayerInput>();
        Actions = new InputActions();
        Actions.Enable();

        initBlindScale = GrowthBlinder.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // Change the scale of the growth blinder
        GrowthBlinder.transform.localScale = Vector3.MoveTowards(GrowthBlinder.transform.localScale,
                Blinded ? BlindedScale : initBlindScale, Time.deltaTime * BlindSpeed);

        // Change the alpha value of the fade blinder
        FadeBlinder.color = new Color(FadeBlinder.color.r, FadeBlinder.color.g, FadeBlinder.color.b,
            Mathf.MoveTowards(FadeBlinder.color.a, Blinded ? 1 : 0, Time.deltaTime * BlindSpeed));
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
