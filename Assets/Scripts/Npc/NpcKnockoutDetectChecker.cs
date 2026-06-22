using UnityEngine;

public class NpcKnockoutDetectChecker : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverMenu;
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            // if not player, exit early ..
            return;
        }
        // Debug.Log($"{other.gameObject.name} is in radius {gameObject.name}. Has player knocked out any player? {GameStateInformation.Instance.HasKnockeOutdNpc}" );
        if (GameStateInformation.Instance.HasKnockeOutdNpc)
        {
            // the player has done something bad ...
            Time.timeScale = 0f;
            // game-over
            GameStateInformation.Instance.IsGameOver = true;
            // show the menu ...
            _gameOverMenu.SetActive(true);
        }
    }
}
