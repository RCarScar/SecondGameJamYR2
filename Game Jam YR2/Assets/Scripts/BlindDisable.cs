using UnityEngine;

public class BlindDisable : MonoBehaviour
{
    public MonoBehaviour Target;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.OpenEyesStartEvent.AddListener(On);
        GameManager.CloseEyesVisualEvent.AddListener(Off);
    }

    void On()
    {
        Target.enabled = true;
    }

    void Off()
    {
        Target.enabled = false;
    }
}
