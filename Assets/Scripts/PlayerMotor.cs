using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Doesn't handle input
[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0f;

    private Rigidbody rb;

    [SerializeField]
    private Camera cam;
    private Vector3 thrusterForce;

    #region Unity Callbacks
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }
    #endregion

    public void Move(Vector3 vel)
    {
        velocity = vel;
    }


    private void PerformMovement()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        if (thrusterForce != Vector3.zero)
        {
            rb.AddForce(thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }
    private void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if (cam)
        {
            currentCameraRotationX += cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -85f, 85f);

            cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        }
    }

    internal void Rotate(Vector3 rotVec)
    {
        rotation = rotVec;
    }

    internal void RotateCamera(float camRotX)
    {
        cameraRotationX = camRotX;
    }

    internal void ApplyThruster(Vector3 thrusterVec)
    {
        thrusterForce = thrusterVec;

    }
}
