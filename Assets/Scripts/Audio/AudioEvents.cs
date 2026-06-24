using FMODUnity;

using UnityEngine;

public class AudioEvents : MonoBehaviour
{
    [field: SerializeField]
    [field: Header("Festival Ambient Noise")]
    public EventReference FestivalNoise { get; private set; }

    [field: Header("Objective SFX")]
    [field: SerializeField]
    public EventReference ObjectiveCompleted { get; private set; }

    [field: Header("Objective Knockout Temple Guard SFX")]
    [field: SerializeField]
    public EventReference ObjectiveKnockoutTempleGuardCompleted { get; private set; }

    [field: Header("Player Walking SFX")]
    [field: SerializeField]
    public EventReference PlayerWalking { get; private set; }


    private static AudioEvents _instance;
    public static AudioEvents Instance
    {
        get
        {
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("Somehow found more than one Audio event in the scene.");
        }
        _instance = this;
    }
}
