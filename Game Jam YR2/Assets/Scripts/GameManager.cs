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
    [Tooltip("Invokes when the player takes damage")] public static UnityEvent PlayerDeathEvent = new UnityEvent();

    [Tooltip("Invoked when Open Eyes animation starts")] public static UnityEvent OpenEyesStartEvent = new UnityEvent();
    [Tooltip("Invoked when Close Eyes animation starts")] public static UnityEvent CloseEyesStartEvent = new UnityEvent();
    [Tooltip("Invoked when the player can see properly")] public static UnityEvent OpenEyesVisualEvent = new UnityEvent();
    [Tooltip("Invoked when the player is sufficiently blinded")] public static UnityEvent CloseEyesVisualEvent = new UnityEvent();
    [Tooltip("Invoked when player starts respawning")] public static UnityEvent PlayerRespawnEvent = new UnityEvent();

    public Blinder Blinder;

    public SpriteRenderer GrowthBlinder;
    public SpriteRenderer FadeBlinder;
    public Vector3 BlindedScale = Vector3.zero;
    public float BlindSpeed = 10;

    public List<Checkpoint> Checkpoints;
    public static Checkpoint CurrentCheckpoint { get; private set; }
    public static int CurrentCheckPointIndex { get; private set; }


    // ----------------- Game State ----------------- //
    public bool Blinded = false;
    public bool Respawning = false;

    // --------------- Blinder Data ---------------- //
    private Vector3 initBlindScale = Vector3.one;

    private void Reset()
    {
        if (Checkpoints == null) Checkpoints = new List<Checkpoint>();
    }

    void Awake()
    {
        Instance = this; //setup static GM instance

        // - setup input - //
        pInput = GetComponent<PlayerInput>();
        Actions = new InputActions();
        Actions.Enable();

        ScreenShake = GetComponent<ScreenShakeManager>();
        AudioPlayer = GetComponent<SoundPlayer>();

        initBlindScale = GrowthBlinder.transform.localScale;

        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        PlayerRespawnEvent.AddListener(RespawnPlayer);
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

    public void PlayerDeath()
    {
        Blinder.Blink();
        PlayerDeathEvent.Invoke();
    }

    public void RespawnPlayer()
    {
        var Player = PlayerManager.Instance; //shorthand
        Player.transform.position = (Vector2) CurrentCheckpoint.transform.position + CurrentCheckpoint.SpawnOffset;
    }

    /// <summary>
    /// Claim the checkpoint at index and all the ones before it
    /// </summary>
    /// <param name="index"></param>
    public static void ClaimCheckpoint(int index)
    {
        if (Instance.Blinded || Instance.Respawning) return;
        var cp = Instance.Checkpoints[index];
        cp.Claimed = true;
        CurrentCheckpoint = cp;
        for (int i = 0; i < index; i++)
        {
            Instance.Checkpoints[i].Claimed = true;
        }
    }

    /// <summary>
    /// Claim the checkpoint and all the ones before it 
    /// <br></br>
    /// > Slower than using an index
    /// </summary>
    /// <param name="cp"></param>
    public static void ClaimCheckPoint(Checkpoint cp)
    {
        if (cp.Claimed || Instance.Blinded || Instance.Respawning) return;
        int i = Instance.Checkpoints.IndexOf(cp);
        if (i >= 0) ClaimCheckpoint(i);
        else throw new System.Exception("Attempted to claim unknown checkpoint.");
    }

    #region Auto-Config Checkpoints
    public bool AddCheckpoint(Checkpoint cp)
    {
        if (!Checkpoints.Contains(cp))
        {
            Checkpoints.Add(cp);
            return true;
        }
        return false;
    }

    public bool RemoveCheckpoint(Checkpoint cp)
    {
        if (Checkpoints.Contains(cp))
        {
            Checkpoints.Remove(cp);
            return true;
        }
        return false;
    }
    #endregion
}
