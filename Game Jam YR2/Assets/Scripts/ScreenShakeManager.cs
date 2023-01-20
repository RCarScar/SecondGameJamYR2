using Cinemachine;
using UnityEditor.Presets;
using UnityEngine;

public class ScreenShakeManager : MonoBehaviour
{
    public static CinemachineImpulseSource PlayImpulse(Preset source, Vector2 position, float force = 1, Transform parent = null)
    {
        if (!source.GetTargetFullTypeName().Equals("Cinemachine.CinemachineImpulseSource")) throw new System.Exception(source.GetTargetTypeName() + " is not an impulse source.");
        GameObject obj = new GameObject("Impulse Source");
        obj.transform.position = position;
        CinemachineImpulseSource src = obj.AddComponent<CinemachineImpulseSource>();

        source.ApplyTo(src);
        src.GenerateImpulse(force);
        Destroy(obj, src.m_ImpulseDefinition.m_ImpulseDuration);
        return src;
    }
}

