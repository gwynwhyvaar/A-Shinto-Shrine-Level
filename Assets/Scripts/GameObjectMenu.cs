using UnityEngine;

public class GameObjectMenu : MonoBehaviour
{
    [SerializeField] private GameObject _gameObjectMenu;
    [SerializeField] private GameObject _gameObjectDialogMenuGameObject;
    [SerializeField] private TMPro.TMP_Text _gameObjectDialogHeader;
    [SerializeField] private TMPro.TMP_Text _gameObjectDialogText;
    [SerializeField] private string _itemDisplayName;
    [SerializeField] private bool _isEndGame = false;
    [SerializeField] private GameObject _pointLight;

    private string _currentItemDisplayName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _gameObjectMenu.SetActive(false);
        _gameObjectDialogMenuGameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Debug.Log($"{_itemDisplayName} triggered {KeyCode.F} for {_currentItemDisplayName}");
            // set the header text ..
            // if (_itemDisplayName == "Abbot_01 " || gameObject.name == "Abbot_01 ")
            // Debug.Log($"Player pressed the F key");
            if (_itemDisplayName == _currentItemDisplayName)
            {
                // hide the action button ..
                _gameObjectMenu.SetActive(false);
                // display text bubble ...
                _gameObjectDialogMenuGameObject.SetActive(true);
                // setting dialog values for game object ..
                // Debug.Log($"Setting dialog values for game object {_currentItemDisplayName}.");
                _gameObjectDialogText.text = GlobalInfo.Instance.GetNpcDialog(_itemDisplayName);
                _gameObjectDialogHeader.text = GlobalInfo.Instance.GetNpcDialogHeader(_itemDisplayName);
                if (_pointLight != null)
                    // turn it off ..
                    _pointLight.SetActive(false);
            }
        }

        // exit npc dialog ...
        if (Input.GetKeyDown(KeyCode.Z) && _itemDisplayName == _currentItemDisplayName)
        {
            _currentItemDisplayName = string.Empty;
            _gameObjectDialogText.text = string.Empty;
            _gameObjectDialogHeader.text = string.Empty;
            // kill text bubble ...
            _gameObjectDialogMenuGameObject.SetActive(false);
            // check if it's end game ..
            if (_isEndGame)
            {
                // refresh mission list ..
                GameStateInformation.Instance.SetupMissions();
                // end the game ..
                GameStateInformation.Instance.IsGameOver = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _currentItemDisplayName = gameObject.name;
        var methodName = $"{nameof(OnTriggerEnter)} - {gameObject.name}";
        // Debug.Log($"{methodName}: Started.");
        // Debug.Log($"{methodName}: Collided with {other.gameObject.name}");
        if (_gameObjectMenu != null)
            if (other.gameObject.CompareTag("Player"))
                _gameObjectMenu.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        _currentItemDisplayName = string.Empty;
        // kill any menu that is active ...
        var methodName = $"{nameof(OnTriggerExit)} - {gameObject.name}";
        // Debug.Log($"{methodName}: Started.");
        if (_gameObjectMenu != null)
            if (other.gameObject.CompareTag("Player"))
                _gameObjectMenu.SetActive(false);
        if (_gameObjectDialogMenuGameObject != null)
            if (other.gameObject.CompareTag("Player"))
            {
                _gameObjectDialogText.text = string.Empty;
                _gameObjectDialogHeader.text = string.Empty;
                // kill text bubble ...
                _gameObjectDialogMenuGameObject.SetActive(false);
            }
    }
}