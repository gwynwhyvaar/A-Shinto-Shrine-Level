using TMPro;
using UnityEngine;

public class MissionWaypoints : MonoBehaviour
{
    [SerializeField] private RectTransform _wayPointPrefab;

    private RectTransform _waypoint;
    private Transform _player;
    private TMP_Text _distanceText;

    private Vector3 _dislpayTextOffset = new(0, 1.25f, 0);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // get the player ..
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        // get the canvas ..
        var canvas = GameObject.Find("Canvas").transform;
        _waypoint = Instantiate(_wayPointPrefab, canvas.transform);
        // get the display text object ..
        _distanceText = _waypoint.GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    private void Update()
    {
        // only display if objective's are available ..
        if (!GameStateInformation.Instance.IsLevelStarted)
        {
            _waypoint.gameObject.SetActive(false);
            return;
        }
        
        if (!GameStateInformation.Instance.MissionExists(gameObject.name))
        {
            _waypoint.gameObject.SetActive(false);
            return;
        }

        var screenPosition = Camera.main.WorldToScreenPoint(transform.position + _dislpayTextOffset);
        _waypoint.position = screenPosition;
        // set the distance text to display ..
        _distanceText.text = $"{Vector3.Distance(_player.position, transform.position).ToString("0")} m";
        // only show waypoint when in front of camera ..
        _waypoint.gameObject.SetActive(screenPosition.z > 0);
    }
}