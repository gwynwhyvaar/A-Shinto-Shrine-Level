using UnityEngine;
/// <summary>
/// this camera ensures the mini -map camera follows and focus' on the player 
/// </summary>
public class LimitCamera : MonoBehaviour
{
    [SerializeField] GameObject _player;
   void LateUpdate(){
       transform.position =new Vector3(_player.transform.position.x, 40, _player.transform.position.z);
      }
}
