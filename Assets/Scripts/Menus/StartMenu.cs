using Constants;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _missionMenu;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _miniMap;
    [SerializeField] private GameObject _endGameMenu;
    [SerializeField] private TMP_Text _missionText1, _missionText2;

    [SerializeField] private TMP_Text _titleText1, _titleText2;

    private bool _isPaused;

    private void Start()
    {
        // hide yhe mini -map ..
        _miniMap.SetActive(false);
        // hide the endgame panel ..
        _endGameMenu.SetActive(false);
        // display the main menu on start ..
        _mainMenu.SetActive(true);
        // disable the mission menu
        _missionMenu.SetActive(false);
        // disable pause /escape menu
        _pauseMenu.SetActive(false);
        // freeze movement 
        Time.timeScale = 0f;
    }

    private void Update()
    {
        var methodCaller = gameObject.name;
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(GameStateInformation.Instance.PrintAllMissions());
            if (_endGameMenu.activeSelf)
            {
                _endGameMenu.SetActive(false);
                RestartGame();
            }
            Debug.Log($"{methodCaller} - in DismissMenu ...");
            if (_mainMenu != null)
            {
                Debug.Log($"{methodCaller} - dismissing menu {_mainMenu.name} ...");
                _mainMenu.SetActive(false);
                // un-freeze movement
                Time.timeScale = 1f;
            }
            if (_missionMenu != null)
            {
                if (_missionMenu.activeSelf) return;
                Debug.Log($"Dismissing menu {_missionMenu.name} ...");
                _missionMenu.SetActive(true);
                // set the missions in the menu 
                var mission1TitleText = GameStateInformation.Instance.GetMissionTitle(MissionConstants.MISSION1_TAG);
                var mission2TitleText = GameStateInformation.Instance.GetMissionTitle(MissionConstants.MISSION2_TAG);
                var mission1Text = GameStateInformation.Instance.GetMissionText(MissionConstants.MISSION1_TAG);
                var mission2Text = GameStateInformation.Instance.GetMissionText(MissionConstants.MISSION2_TAG);
                // set the texts
                // title ..
                _titleText1.text = $"{_titleText1.text} {mission1TitleText}";
                _titleText2.text = $"{_titleText2.text} {mission2TitleText}";
                // body ..
                _missionText1.text = $"{mission1Text}";
                _missionText2.text = $"{mission2Text}";
                // start the elevel ..
                GameStateInformation.Instance.IsLevelStarted = true;
            }

            if (_miniMap != null)
            {
                _miniMap.SetActive(true);
            }
        }

        // toggle the mission menu from display ...
        if (Input.GetKeyDown(KeyCode.H) && GameStateInformation.Instance.IsLevelStarted)
        {
            if (_missionMenu.activeSelf)
                _missionMenu.SetActive(false);
            else
                _missionMenu.SetActive(true);
        }

        // toggle the mini-map menu from display ...
        if (Input.GetKeyDown(KeyCode.M) && GameStateInformation.Instance.IsLevelStarted)
        {
            if (_miniMap.activeSelf)
                _miniMap.SetActive(false);
            else
                _miniMap.SetActive(true);
        }

        // toggle pause menu ..
        if (GameStateInformation.Instance.IsLevelStarted && Input.GetKeyDown(KeyCode.P))
        {
            if (GameStateInformation.Instance.IsGameOver) RestartGame();
            if (_pauseMenu.activeSelf && _isPaused)
            {
                _isPaused = false;
                _pauseMenu.SetActive(false);
                // un -freeze movement 
                Time.timeScale = 1f;
            }
            else
            {
                if (GameStateInformation.Instance.IsGameOver) return;
                _isPaused = true;
                _pauseMenu.SetActive(true);
                // freeze movement 
                Time.timeScale = 0f;
            }
        }

        // handle pause menu interaction ..
        if (_isPaused && GameStateInformation.Instance.IsLevelStarted)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                // quit the game ..
                Debug.Log($"Exiting game ...");
#if UNITY_EDITOR
                // Application.Quit() does not work in the editor so
                // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
                UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
            }

            if (Input.GetKeyDown(KeyCode.R)) RestartGame();
        }

        // check if player has completed game ..
        if (GameStateInformation.Instance.IsGameOver)
        {
            // hide everything 
            _mainMenu.SetActive(false);
            // disable the mission menu
            _missionMenu.SetActive(false);
            // enable the mini-map
            _miniMap.SetActive(false);
            // disable pause /escape menu
            _pauseMenu.SetActive(false);
            // freeze movement 
            Time.timeScale = 0f;
            // show end game panel 
            _endGameMenu.SetActive(true);
        }
    }

    // Update is called once per frame
    public void DismissMenu()
    {
        // Debug.Log("in DismissMenu ...");
        if (_mainMenu != null) _mainMenu.SetActive(false);
    }

    private void RestartGame()
    {
        // restart the game ..
        Debug.Log($"Restarting game ...");
        // it's no longer gamer ..
        GameStateInformation.Instance.IsGameOver = false;
        // update the player's knock-out status and other things ..
        GameStateInformation.Instance.IsLevelStarted = false;
        GameStateInformation.Instance.HasKnockeOutdNpc = false;
        // restarting the scene ..
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}