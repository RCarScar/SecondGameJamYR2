using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlatformEffector2D))]
public class OneWayPlatform : MonoBehaviour
{
    PlatformEffector2D m_PlatformEffector;

    bool flipped = false;

    // Start is called before the first frame update
    void Start()
    {
        m_PlatformEffector = GetComponent<PlatformEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        m_PlatformEffector.rotationalOffset = flipped ? 180 : 0;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Player") && GameManager.Actions.Game.Fall.IsPressed())
        {
            flipped = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        flipped = false;
    }
}
