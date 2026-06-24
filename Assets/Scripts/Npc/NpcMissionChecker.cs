using UnityEngine;

public class NpcMissionChecker : MonoBehaviour
{
    [SerializeField] private string _missionTag;
    [SerializeField] private int _minimumPassCount = 0;
    [SerializeField] private bool _checkInDisguise; // = 0;

    private void Update()
    {
        if (GameStateInformation.Instance.GetMissionCount(_missionTag) <= _minimumPassCount)
            gameObject.SetActive(false);

        if (_checkInDisguise)
            if (GameStateInformation.Instance.IsInDisguise)
                gameObject.SetActive(false);
    }
}