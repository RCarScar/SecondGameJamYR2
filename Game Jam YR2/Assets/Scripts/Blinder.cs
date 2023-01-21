/* Made by Blake Rubadue */

using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animation))]
public class Blinder : MonoBehaviour
{
    [SerializeField] AnimationClip CloseEyeClip;
    [SerializeField] AnimationClip OpenEyeClip;
    [SerializeField] AnimationClip BlinkClip;

    Animation anim;

    private void Start()
    {
        anim = GetComponent<Animation>();
        anim.AddClip(OpenEyeClip, "Open");
        anim.AddClip(CloseEyeClip, "Close");
        anim.AddClip(BlinkClip, "Blink");
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

    public void InvokeOpenEyes()
    {
        GameManager.OpenEyesEvent.Invoke();
        Debug.Log("Open");
    }

    public void InvokeCloseEyes()
    {
        GameManager.CloseEyesEvent.Invoke();
        Debug.Log("Close");
    }

    public void InvokeBlink()
    {
        GameManager.BlinkEvent.Invoke();
        Debug.Log("Blink");
    }
}
