/* Made by Blake Rubadue */

using UnityEditor.Presets;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    public static PlayerController Controller { get; private set; }

    public Preset LandShake;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        Controller = GetComponent<PlayerController>();

        SubscribeEvents();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Damager"))
        {
            GameManager.Instance.PlayerDeath();
        }
    }

    void SubscribeEvents()
    {
        Controller.LandEvent.AddListener(OnLand);
    }

    void OnLand()
    {
        ScreenShakeManager.PlayImpulse(LandShake, transform.position);
    }
}
