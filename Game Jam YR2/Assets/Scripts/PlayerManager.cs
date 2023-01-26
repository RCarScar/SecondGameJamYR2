/* Made by Blake Rubadue */

using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    public static PlayerController Controller { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        Controller = GetComponent<PlayerController>();

        SubscribeEvents();
    }

    private void FixedUpdate()
    {
        bool ignoreDamage = GameManager.Instance.Blinded || GameManager.Instance.Respawning;
        Physics2D.IgnoreLayerCollision(3, 6, ignoreDamage); //ignore collision between player and enemy?
        Physics2D.IgnoreLayerCollision(3, 7, ignoreDamage); //ignore collision between player and hazards?
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damager"))
        {
            GameManager.Instance.PlayerDeath();
        }
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
        //ScreenShakeManager.PlayImpulse(LandShake, transform.position);
    }
}
