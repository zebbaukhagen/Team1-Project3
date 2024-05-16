using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boatControls : MonoBehaviour
{
    /// <summary>
    /// rotate the boat at a static speed, either  clockwise or counter-clockwise depending on player input
    /// </summary>

    [SerializeField] private GameObject sail;
    [SerializeField] private PlayerUI playerUI;
    [SerializeField] private WindManager windManager;

    private Quaternion currentBoatRotation;
    private Quaternion currentSailRotation;
    private float rotationSpeed = 75f;
    private float movementMultiplier = 1f;
    private float currentVelocity = 0f;
    [SerializeField]  private float dragCoefficient = 0.05f;

    private void Start()
    {
        windManager = GameObject.Find("WindManager").GetComponent<WindManager>();
        playerUI = GameObject.Find("PlayerUICanv").GetComponent<PlayerUI>();
        currentBoatRotation = transform.rotation;
        currentSailRotation = sail.transform.localRotation;
    }

    private void Update()
    {
        #region Get Input
        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
        {
            // do nothing
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateBoatClockwise();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            RotateBoatCounterClockwise();
        }

        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
        {
            // do nothing
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            RotateSailClockwise();
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            RotateSailCounterClockwise();
        }
        #endregion
    }

    private void FixedUpdate()
    {
        MoveBoat();
    }

    private float CalculatePropulsion(Vector3 windDirection, Quaternion sailRotation, Quaternion boatRotation, float windForce)
    {
        // 1. Sail-Wind relation
        Vector3 eulerRotation = sailRotation.eulerAngles;
        float sailAngle = eulerRotation.y;
        float windAngle = Mathf.Atan2(windDirection.x, windDirection.z) * Mathf.Rad2Deg;
        float sailWindDelta = Mathf.DeltaAngle(windAngle, sailAngle);
        float sailWindEfficiency = MathF.Abs(Mathf.Sin(sailWindDelta * Mathf.Deg2Rad));

        // 2. Sail-Boat relation
        Vector3 boatForward = boatRotation * Vector3.forward;
        Vector3 sailDirection = sailRotation * Vector3.forward;
        float sailBoatDelta = Vector3.Angle(boatForward, sailDirection);
        float sailBoatEfficiency = Mathf.Sin(sailBoatDelta * Mathf.Deg2Rad);

        // 3. Boat-Wind relation
        float boatWindDelta = Vector3.Angle(-boatForward, windDirection);

        float boatWindEfficiency;
        if (boatWindDelta == 0f)
        {
            boatWindEfficiency = 0f;
        }
        else if (boatWindDelta <= 90f)
        {
            boatWindEfficiency = boatWindDelta / 90f;
        }
        else
        {
            boatWindEfficiency = 1f;
        }

        // Combining efficiencies
        float totalEfficiency = sailWindEfficiency * sailBoatEfficiency * boatWindEfficiency;
        float propulsion = windForce * totalEfficiency;

        playerUI.SetPower(propulsion);

        return propulsion;
    }




    private void MoveBoat()
    {
        float propulsion = CalculatePropulsion(windManager.m_currentWindDirection, currentSailRotation, transform.rotation, windManager.m_currentWindForce) * movementMultiplier;

        // Update current velocity with the propulsion
        currentVelocity += propulsion * Time.deltaTime;

        // Apply drag
        currentVelocity = currentVelocity - (dragCoefficient * currentVelocity * Time.deltaTime);

        // Assuming currentBoatRotation is the forward direction vector of the boat
        Vector3 movementDirection = currentBoatRotation * Vector3.forward;
        transform.position += currentVelocity * Time.deltaTime * movementDirection;
    }

    #region Control Functions
    private void RotateBoatCounterClockwise()
    {
        // rotate the player counter clockwise
        currentBoatRotation *= Quaternion.Euler(0f, -rotationSpeed * Time.deltaTime, 0f);
        transform.rotation = currentBoatRotation;
        currentSailRotation = sail.transform.localRotation;
    }

    private void RotateBoatClockwise()
    {
        // rotate the player clockwise
        currentBoatRotation *= Quaternion.Euler(0f, rotationSpeed * Time.deltaTime, 0f);
        transform.rotation = currentBoatRotation;
        currentSailRotation = sail.transform.localRotation;
    }

    private void RotateSailCounterClockwise()
    {
        // rotate the sail counter clockwise
        currentSailRotation *= Quaternion.Euler(0f, rotationSpeed * Time.deltaTime, 0f);
        sail.transform.localRotation = currentSailRotation;
    }

    private void RotateSailClockwise()
    {
        // rotate the sail clockwise
        currentSailRotation *= Quaternion.Euler(0f, -rotationSpeed * Time.deltaTime, 0f);
        sail.transform.localRotation = currentSailRotation;
    }

    #endregion
}
