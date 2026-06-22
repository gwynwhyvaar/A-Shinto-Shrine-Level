using System.Collections.Generic;

using FMOD.Studio;

using FMODUnity;

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private List<EventInstance> _eventInstances;
    private List<StudioEventEmitter> _studioEvents;

    // private EventInstance _musicEventInstance;

    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            return _instance;
        }
    }
    private void Start()
    {
     //   InitialiseBackgroundMusic(AudioEvents.Instance.BackgroundMusic);
    }
    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("Somehow found more than one Audio manager in the scene.");
        }
        _instance = this;
        _eventInstances = new List<EventInstance>();
        _studioEvents = new List<StudioEventEmitter>();
    }
    public void PlayOneShot(EventReference sound, Vector3 worldPosition)
    {
        RuntimeManager.PlayOneShot(sound, worldPosition);
    }
    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        _eventInstances.Add(eventInstance);
        return eventInstance;
    }
    private void CleanUp()
    {
        // stop and release any created instances
        if (_eventInstances.Count > 0)
        {
            _eventInstances.ForEach(x =>
            {
                x.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                x.release();
            });
        }
        // stop all of the event emitter, so they don't hang around other scenes..
        if (_studioEvents.Count > 0)
        {
            _studioEvents.ForEach(x =>
            {
                x.Stop();
            });
        }
    }
    private void OnDestroy() => CleanUp();

    public StudioEventEmitter InitialiseStudioEventEmitter(EventReference eventReference, GameObject gameObject)
    {
        StudioEventEmitter emitter = gameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;

        _studioEvents.Add(emitter);
        return emitter;
    }
    public void Update()
    {
        // _musicEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
    }
    private void InitialiseBackgroundMusic(EventReference backgroundMusicReference)
    {
        /*_musicEventInstance = CreateInstance(backgroundMusicReference);
        // _musicEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        RuntimeManager.AttachInstanceToGameObject(_musicEventInstance, gameObject, gameObject);

        _musicEventInstance.start();*/
    }
}
