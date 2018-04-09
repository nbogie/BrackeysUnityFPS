﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles the input and sends commands to the motor
[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    // Component caching
    private PlayerMotor motor;
    private ConfigurableJoint joint;
    private Animator animator;

    [Header("Controls")]
    [SerializeField]
    private float moveSpeed = 8f;
    [SerializeField]
    private float lookSensitivity = 8f;
    [SerializeField]
    private float lookSensitivityVertical = 3f;

    [SerializeField]
    private float yPositionSpring = 10f;

    [Header("Jetpac Settings")]
    [SerializeField]
    private float thrusterForce = 1000f;
    [SerializeField]
    private float fuelPerFrame = 1f;
    [SerializeField]
    private float fuelRegenPerFrame = 1f;
    private const float maxFuel = 100f;
    [SerializeField]
    private float fuelRemaining = maxFuel;

    #region Unity Callbacks

    void Start()
    {
        joint = GetComponent<ConfigurableJoint>();
        motor = GetComponent<PlayerMotor>();
        animator = GetComponent<Animator>();
        fuelRemaining = maxFuel;

    }

    void Update()
    {
        //Movement of sphere
        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");
        Vector3 movHorizontal = transform.right * xMove;
        Vector3 movForward = transform.forward * zMove;
        Vector3 vel = (movHorizontal + movForward) * moveSpeed;

        animator.SetFloat("ForwardSpeed", zMove);

        motor.Move(vel);

        Vector3 thrusterVec = Vector3.zero;
        if (Input.GetButton("Jump") && HasFuel())
        {
            thrusterVec = Vector3.up * thrusterForce;
            SetSpringActive(false);
            SpendFuel();
        }
        else
        {
            SetSpringActive(true);
            RegenerateFuel();
        }
        motor.ApplyThruster(thrusterVec);

        //Turning the sphere
        float yRot = Input.GetAxis("Mouse X");
        Vector3 rotVec = new Vector3(0, yRot, 0) * lookSensitivity;
        motor.Rotate(rotVec);

        //Rotating the camera
        float xRotInc = Input.GetAxis("Mouse Y");
        motor.RotateCamera(-xRotInc * lookSensitivityVertical);

        if (Input.GetKeyDown(KeyCode.K)){
            Debug.Log("Killing player by keypress " + transform.name);
            GetComponent<Player>().RpcTakeDamage(99999);
        }
    }

    #endregion

    void SetSpringActive(bool state)
    {
        JointDrive yDriveSettings = new JointDrive();
        yDriveSettings.positionSpring = state ? yPositionSpring : 0f;
        yDriveSettings.positionDamper = 0;
        yDriveSettings.maximumForce = 100f;
        joint.yDrive = yDriveSettings;

    }
    private void SpendFuel()
    {
        fuelRemaining -= fuelPerFrame;
    }

    private bool HasFuel()
    {
        return fuelRemaining > 0f;
    }

    void RegenerateFuel()
    {
        fuelRemaining += fuelRegenPerFrame;
        if (fuelRemaining > maxFuel)
        {
            fuelRemaining = maxFuel;
        }

    }
}
