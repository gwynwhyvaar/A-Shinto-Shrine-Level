using System.Collections;
using Constants;
using UnityEngine;

public class NpcMenu : MonoBehaviour
{
    [SerializeField] private GameObject _npcMenu;
    [SerializeField] private GameObject _npcDialogMenuGameObject;
    [SerializeField] private TMPro.TMP_Text _npcDialogHeader;
    [SerializeField] private TMPro.TMP_Text _npcDialogText;
    [SerializeField] private string _npcDisplayName;
    [SerializeField] private bool _isTempleGuard;
    [SerializeField] private GameObject _pointLight;

    private string _currentNpcDisplayName;
    private bool _isNpcKnockedOut;

    private void Start()
    {
        _npcMenu.SetActive(false);
        _npcDialogMenuGameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            // kill the popup menu ...
            _npcMenu.SetActive(false);
            // set the header text ..
            if (_npcDisplayName.Equals(_currentNpcDisplayName))
            {
                // display text bubble ...
                _npcDialogMenuGameObject.SetActive(true);
                _npcDialogText.text = GlobalInfo.Instance.GetNpcDialog(_npcDisplayName);
                _npcDialogHeader.text = GlobalInfo.Instance.GetNpcDialogHeader(_npcDisplayName);
            }
        }

        // knock -out the npc ...
        if (Input.GetKeyDown(KeyCode.K) && _currentNpcDisplayName != null &&
            _currentNpcDisplayName.Equals(_npcDisplayName))
        {
            // remove the menu ..
            _npcMenu.SetActive(false);
            if (!_isTempleGuard)
            {
                // display text bubble ...
                _npcDialogMenuGameObject.SetActive(true);
                _npcDialogText.text =
                    MissionConstants.NOT_TEMPLE_GUARD_REBUKE; // GlobalInfo.Instance.GetNpcDialog(_npcDisplayName);
                _npcDialogHeader.text = GlobalInfo.Instance.GetNpcDialogHeader(_npcDisplayName);
            }
            else
            {
                Debug.Log(
                    $"{nameof(_npcMenu)} - setting in-disguise to true for : {gameObject.transform.gameObject.name}");
                // turn off point light ..
                if (_pointLight != null) _pointLight.SetActive(false);
                // display text bubble ...
                _npcDialogMenuGameObject.SetActive(true);
                _npcDialogText.text =
                    "You have knocked out the monk and donned his robes as a disquise. Now you can move freely in the temple."; // MissionConstants.NOT_TEMPLE_GUARD_REBUKE; // GlobalInfo.Instance.GetNpcDialog(_npcDisplayName);
                _npcDialogHeader.text = GlobalInfo.Instance.GetNpcDialogHeader(_npcDisplayName);
                // start co-routine ..
                // StartCoroutine(KillDialog(_npcDialogMenuGameObject));
                // set in disguise ..
                GameStateInformation.Instance.IsInDisguise = true;
                // update the player's knocked out status ..
                GameStateInformation.Instance.HasKnockeOutdNpc = true;
                // kill this object ..
                Destroy(gameObject.transform.parent.gameObject, 2f);
            }
        }

        // exit npc dialog ...
        if (Input.GetKeyDown(KeyCode.Z))
        {
            _currentNpcDisplayName = string.Empty;
            _npcDialogText.text = string.Empty;
            _npcDialogHeader.text = string.Empty;
            // kill text bubble ...
            _npcDialogMenuGameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _currentNpcDisplayName = gameObject.name;
        var methodName = $"{nameof(OnTriggerEnter)} - {gameObject.name}";
        Debug.Log($"{methodName}: Collided with {other.gameObject.name}");
        if (_npcMenu != null)
            if (other.gameObject.CompareTag("Player"))
            {
                GameStateInformation.Instance.HasKnockeOutdNpc = false;
                _npcMenu.SetActive(true);
            }
    }

    private IEnumerator KillDialog(GameObject dialog)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(dialog);
    }

    private void OnTriggerExit(Collider other)
    {
        _currentNpcDisplayName = string.Empty;
        // refresh the knocked out status
        GameStateInformation.Instance.HasKnockeOutdNpc = false;
        // kill any menu that is active ...
        if (_npcMenu != null)
            if (other.gameObject.CompareTag("Player"))
                _npcMenu.SetActive(false);
        if (_npcDialogMenuGameObject != null)
            if (other.gameObject.CompareTag("Player"))
            {
                _npcDialogText.text = string.Empty;
                _npcDialogHeader.text = string.Empty;
                _npcDialogMenuGameObject.SetActive(false);
            }
    }
}