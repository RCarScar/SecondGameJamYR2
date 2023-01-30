using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct ZonePermissions
{
    public LayerMask mask;
    public string[] tags;
}

[ExecuteAlways]
public class Zone : MonoBehaviour
{
    public ZonePermissions Permissions;

    [Tooltip("Invoked when a permitted object enters the zone.")]
    public UnityEvent ZoneEnterEvent = new();

    [Tooltip("Invoked when a permitted object exits the zone")]
    public UnityEvent ZoneExitEvent = new();

    [Tooltip("Invoked every frame a permitted object is in the zone. Gives data about the zone.")]
    public UnityEvent<Zone> ZoneStayEvent = new();

    public bool run = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    #region Config

    private void OnValidate()
    {
        if (run)
        {
            ZoneEnterEvent.Invoke();
            run = false;
        }
        #region Force Config Collider
        if (GetComponent<Collider2D>() == null) 
        { 
            gameObject.AddComponent<BoxCollider2D>();
            Debug.LogWarning("Zone depends on Collider2D, added BoxCollider2D to fulfill dependency.", this);
        }
        GetComponent<Collider2D>().isTrigger = true;
        #endregion
    }
    #endregion
}
