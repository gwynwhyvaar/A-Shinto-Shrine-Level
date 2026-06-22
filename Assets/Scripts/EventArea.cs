using FMODUnity;

using UnityEngine;

[RequireComponent(typeof(StudioEventEmitter))]
public class EventArea : MonoBehaviour
{
    private StudioEventEmitter _emitter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _emitter = AudioManager.Instance.InitialiseStudioEventEmitter(AudioEvents.Instance.FestivalNoise, this.gameObject);
        _emitter.Play();
    }
}
