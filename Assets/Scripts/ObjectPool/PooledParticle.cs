using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class PooledParticle : MonoBehaviour
{

    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            ParticleSystem particle = GetComponent<ParticleSystem>();
#if UNITY_EDITOR
            Undo.RecordObject(particle,"ParticleCallbackChange");
#endif
            var module = particle.main;
            module.stopAction = ParticleSystemStopAction.Callback;
                
#if UNITY_EDITOR
            EditorUtility.SetDirty(particle);
#endif
        }
    }

    private void Start()
    {
        ParticleSystem particle = GetComponent<ParticleSystem>();
        var module = particle.main;
        module.stopAction = ParticleSystemStopAction.Callback;
    }

    private void OnParticleSystemStopped()
    {
        ObjectPoolManager.ReturnObjectToPool(this.gameObject);
    }
}