/* Made by Blake Rubadue */

using UnityEngine;
using UnityEngine.Events;
using System.Reflection;

[RequireComponent(typeof(Animation))]
public class Blinder : MonoBehaviour
{
    [SerializeField] AnimationClip CloseEyeClip;
    [SerializeField] AnimationClip OpenEyeClip;
    [SerializeField] AnimationClip BlinkClip;

    public UnityEvent TestEvent = new UnityEvent();

    Animation anim;

    private void Start()
    {
        anim = GetComponent<Animation>();
        anim.AddClip(OpenEyeClip, "Open");
        anim.AddClip(CloseEyeClip, "Close");
        anim.AddClip(BlinkClip, "Blink");
        TestEvent.AddListener(InvokeRespawnStart);
    }

    public void OpenEyes()
    {
        anim.Play("Open");
    }

    public void CloseEyes()
    {
        anim.Play("Close");
    }

    public void Blink()
    {
        anim.Play("Blink");
    }

    public void InvokeRespawnStart()
    {
        GameManager.PlayerRespawnEvent.Invoke();
        GameManager.Instance.Respawning = true;
        Debug.Log("Respawn");
    }

    public void InvokeRespawnEnd()
    {
        GameManager.Instance.Respawning = false;
        Debug.Log("Respawn Finished");
    }

    public void InvokeStartOpenEyes()
    {
        GameManager.OpenEyesStartEvent.Invoke();
        Debug.Log("Start Open");
    }

    public void InvokeStartCloseEyes()
    {
        GameManager.CloseEyesStartEvent.Invoke();
        Debug.Log("Start Close");
    }

    public void InvokeOpenEyes()
    {
        GameManager.OpenEyesVisualEvent.Invoke();
        Debug.Log("Open");
    }

    public void InvokeCloseEyes()
    {
        GameManager.CloseEyesVisualEvent.Invoke();
        Debug.Log("Close");
    }
}
