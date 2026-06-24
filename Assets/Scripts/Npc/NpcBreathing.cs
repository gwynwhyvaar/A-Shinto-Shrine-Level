using UnityEngine;

public class NpcBreathing : MonoBehaviour
{
    [SerializeField] private GameObject _targetGameObject;
    [SerializeField] private float _expandDuration = 1.0f;
    [SerializeField] private Vector3 _breatheIn, _breathOut;
    [SerializeField] private bool _isPulsing = false;

    private float _currentTime = 0f;

    private bool _isBreathingIn = true;

    // Start is called before the first frame update
    private void Awake()
    {
        if (!_targetGameObject) _targetGameObject = gameObject;
    }

    // Update is called once per frame
    private void Update()
    {
        BreathUpdate();
    }

    private void BreathUpdate()
    {
        if (_isPulsing)
        {
            var targetScale = _isBreathingIn ? _breatheIn : _breathOut;
            var startScale = _isBreathingIn ? _breathOut : _breatheIn;

            _currentTime += Time.deltaTime;

            var lerpFactor = _currentTime / _expandDuration;

            _targetGameObject.transform.localScale = Vector3.Lerp(startScale, targetScale, lerpFactor);
            if (lerpFactor >= 1.0f)
            {
                _isBreathingIn = !_isBreathingIn;
                _currentTime = 0f;
            }
        }
    }
}