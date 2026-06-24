using UnityEngine;

public class NpcMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _startingPoint;
    [SerializeField] private Transform[] _points;

    private int _index;
    private void Start()
    {
        transform.position = _points[_startingPoint].position;
    }

    // Update is called once per frame
    private void Update()
    {
        MovePosition();
    }

    private void MovePosition()
    {
        Debug.Log($"{nameof(NpcMovement)} - {gameObject.name} moving .. to {_points[_index].name}");
        // Debug.Log("Moving platform ...");
        var distance = Vector3.Distance(transform.position, _points[_index].position);
        // Debug.Log($"Distance: {distance}");
        if (distance < 0.02f) // what is this magic number??
        {
            _index++;
            // Debug.Log($"Increasing Index to {_index}");
            if (_index == _points.Length)
            {
                // reset the index
                _index = 0;
                Debug.Log($"Resetting Index to {_index}");
            }
        }

        // moving the platform to the point position with the index '_index'
        // Debug.Log($"Moved platform to position: {_index}");
        transform.position = Vector3.MoveTowards(transform.position, _points[_index].position, _speed * Time.deltaTime);
    }
}