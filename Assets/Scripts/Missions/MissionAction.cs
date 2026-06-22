using Constants;

using TMPro;

using UnityEngine;

/// <summary>
/// only use this script to handle waypoints/ checkpoints that will trigger or work with mini - missions
/// </summary>
public class MissionAction : MonoBehaviour
{
    [SerializeField] private string _missionTag;
    [SerializeField] private TMP_Text _missionText;
    [SerializeField] private string _mainMissionTag;
    [SerializeField] private bool _isKnockoutAllowed;

    private string _currentGameObject;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && _currentGameObject != null && _currentGameObject == gameObject.name)
        {
            // update the mission catalog ...
            GameStateInformation.Instance.RemoveMissions(gameObject.name, _missionTag);
            // check if minor missions have completed
            if (GameStateInformation.Instance.IsLevelStarted &&
                GameStateInformation.Instance.IsMissionComplete(_missionTag))
            {
                // Debug.Log($"{gameObject.name} completed {_missionTag}. getting Main -mission {_mainMissionTag}");
                // set for the main mission .. 
                var missionText = GameStateInformation.Instance.GetMissionText(_mainMissionTag);
                // set the texts
                _missionText.text = $"{missionText}";
            }
            else
            {
                // Debug.Log($"{gameObject.name} getting mini -mission : {_missionTag}");
                // update the appropriate mission text
                _missionText.text = GameStateInformation.Instance.GetMissionText(_missionTag);
            }

            // play some sound ..
            AudioManager.Instance.PlayOneShot(AudioEvents.Instance.ObjectiveCompleted, this.transform.position);
        }

        if (GameStateInformation.Instance.IsInDisguise && _currentGameObject != null &&
            _currentGameObject == gameObject.name && MissionConstants.MISSION2_TAG == _missionTag && _isKnockoutAllowed)
        {
            Debug.Log($"knocking out : {gameObject.transform.gameObject.name}");
            UpdateMissionStatus(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{nameof(MissionAction)} - {gameObject.name} triggered {_missionTag}");
        _currentGameObject = gameObject.name;
    }

    private void OnTriggerExit(Collider other)
    {
        _currentGameObject = string.Empty;
    }

    private void UpdateMissionStatus(bool isKnockedOut)
    {
        if (GameStateInformation.Instance.MissionExists(_currentGameObject))
        {
            // play some sound .
            if (isKnockedOut)
            {
                AudioManager.Instance.PlayOneShot(AudioEvents.Instance.ObjectiveKnockoutTempleGuardCompleted, this.transform.position);
            }
            else
            {
                AudioManager.Instance.PlayOneShot(AudioEvents.Instance.ObjectiveCompleted, this.transform.position);
            }
            // update the mission catalog ...
            GameStateInformation.Instance.RemoveMissions(gameObject.name, _missionTag);
            // check if minor missions have completed
            if (GameStateInformation.Instance.IsLevelStarted &&
                GameStateInformation.Instance.IsMissionComplete(_missionTag))
            {
                // set for the main mission .. 
                var missionText = GameStateInformation.Instance.GetMissionText(_mainMissionTag);
                // set the texts
                _missionText.text = $"{missionText}";
            }
            else
            {
                // update the appropriate mission text
                _missionText.text = GameStateInformation.Instance.GetMissionText(_missionTag);
            }
        }
    }
}