/*
    ██╗       ██╗ █████╗ ██╗     ██╗       ██████╗ ██╗   ██╗███╗  ██╗
    ██║  ██╗  ██║██╔══██╗██║     ██║       ██╔══██╗██║   ██║████╗ ██║
    ╚██╗████╗██╔╝███████║██║     ██║       ██████╔╝██║   ██║██╔██╗██║
     ████╔═████║ ██╔══██║██║     ██║       ██╔══██╗██║   ██║██║╚████║
     ╚██╔╝ ╚██╔╝ ██║  ██║███████╗███████╗  ██║  ██║╚██████╔╝██║ ╚███║
      ╚═╝   ╚═╝  ╚═╝  ╚═╝╚══════╝╚══════╝  ╚═╝  ╚═╝ ╚═════╝ ╚═╝  ╚══╝

    █▄▄ █▄█   ▀█▀ █ █ █▀▀   █▀▄ █▀▀ █ █ █▀▀ █   █▀█ █▀█ █▀▀ █▀█
    █▄█  █     █  █▀█ ██▄   █▄▀ ██▄ ▀▄▀ ██▄ █▄▄ █▄█ █▀▀ ██▄ █▀▄
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SFPSC_PlayerMovement))]
public class SFPSC_WallRun : MonoBehaviour
{
    private static Vector3 vecZero = Vector3.zero;

    public Transform startingPosition; // The starting position for two raycasts(left and right) to check if there is a wall near

    [Header("Properties")]
    public LayerMask layerMask; // The walls' layers mask that the player is going to attach
    public float maxDistanceToWall = 1.5f; // The maximum distance to attach the player to a wall
    public float constantWallRunForce = 15.0f; // This force is going to be applied on the player constantly while wall running
    public float jumpForce = 600.0f; // The jump force that is applied when you jump while wall running
    public float cameraTiltAngle = 10.0f; // The angle that the camera tilts when wall running
    public float minSpeedWhenAttached = 10.0f;
    public float t1 = 5.0f, multiplier = 4.5f;
    public float jumpWallMultiplier = 0.5f, jumpForwardMultiplier = 0.3f, jumpUpMultiplier = 0.2f;

    [Header("Block times")]
    public float jumpBlockTime = 0.8f; // The jump function is blocked for this amount of seconds
    public float attachToWallBlockTime = 1.0f; // The attach to wall function is blocked for this amount of seconds

    public bool IsWallRunning
    {
        get { return wallRunning; }
    }

    private SFPSC_PlayerMovement pm;
    private Rigidbody rb;
    private void Start()
    {
        pm = this.GetComponent<SFPSC_PlayerMovement>();
        rb = this.GetComponent<Rigidbody>();
    }

    private RaycastHit hitInfo;
    private bool blocked = false, wallRunning = false;
    private void FixedUpdate()
    {
        if (!rb.useGravity || blocked)
            return;

        if(Physics.Raycast(startingPosition.position, transform.right, out hitInfo, maxDistanceToWall, layerMask))
        {
            if (pm.vInput >= .5f)
            {
                if (!wallRunning)
                    StartWallRunning();
                AddForces(hitInfo.normal, true);
                return;
            }
        }
        if(Physics.Raycast(startingPosition.position, -transform.right, out hitInfo, maxDistanceToWall, layerMask))
        {
            if (pm.vInput >= .5f)
            {
                if (!wallRunning)
                    StartWallRunning();
                AddForces(hitInfo.normal, false);
                return;
            }
        }

        if (wallRunning)
            StopWallRunning();
        gravityForce = vecZero;
    }
    
    private float t = 0.0f, mag;
    private void StartWallRunning()
    {
        wallRunning = true;
        t = t1;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, t, rb.linearVelocity.z);
    }

    private void StopWallRunning()
    {
        wallRunning = false;
        SFPSC_FPSCamera.cam.rotZ = 0.0f; // Resetting the z rotation on the camera (if the player shoots and the camera shakes it won't be such a problem)
        blocked = true;
        Invoke(nameof(UnblockWallRunning), attachToWallBlockTime);
    }

    private void UnblockWallRunning()
    {
        blocked = false;
    }
    
    private Vector3 gravityForce;
    private bool isJumpAvailable = true;
    private void AddForces(Vector3 wallNormal, 
        bool right) // if right is false it means that the wall is on the left side
    {
        if (isJumpAvailable && Input.GetKey(SFPSC_KeyManager.Jump))
        {
            rb.AddForce((hitInfo.normal * jumpWallMultiplier + transform.forward * jumpForwardMultiplier + Vector3.up * jumpUpMultiplier).normalized * rb.mass * jumpForce);
            isJumpAvailable = false;
            Invoke(nameof(UnblockJump), jumpBlockTime);
        }

        if (t >= 0.0f)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, t, rb.linearVelocity.z);
            t -= multiplier * Time.fixedDeltaTime;
        }

        mag = new Vector2(rb.linearVelocity.x, rb.linearVelocity.z).magnitude;
        if (mag < minSpeedWhenAttached)
            rb.linearVelocity = new Vector3(rb.linearVelocity.x / mag, rb.linearVelocity.y / minSpeedWhenAttached, rb.linearVelocity.z / mag) * minSpeedWhenAttached;

        // if the player somehow calls Shake(...) method on the FPSCamera script it is going to set the rotZ to 0 and there
        // is no way to inform the player that he is on a wall so we constantly set the rotZ to the angle
        SFPSC_FPSCamera.cam.rotZ = right ? cameraTiltAngle : -cameraTiltAngle;
    }

    private static Vector3 ClampSqrMag(Vector3 vec, float sqrMag)
    {
        if (vec.sqrMagnitude > sqrMag)
            vec = vec.normalized * Mathf.Sqrt(sqrMag);
        return vec;
    }

    private void UnblockJump()
    {
        isJumpAvailable = true;
    }
}
