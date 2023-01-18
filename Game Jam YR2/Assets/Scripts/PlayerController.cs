using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float _Speed = 10f;
    public float _Acceleration = 25f;


    private float vel = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement(Time.deltaTime);
    }

    void HandleMovement(float dt)
    {
        float dir = 1;
        Mathf.MoveTowards(vel, _Speed * dir, _Acceleration * dt);
    }
}
