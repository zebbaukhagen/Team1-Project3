using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.UI;
using System;

public class GateHandler : MonoBehaviour
{
    int completedGateAmount = 0;
    public int missedGateAmount = 0;
    public Gradient gateGradient;
    private AudioSource source;

    public float StandardTime;

    [SerializeField] private AudioClip[] clip;

    [SerializeField] private GameObject[] gates;

    public UnityEvent<bool> StartGateEvent = new UnityEvent<bool>();
    public UnityEvent IterateGateEvent = new UnityEvent();
    public UnityEvent EndGateEvent = new UnityEvent();

    

    public void Start()
    {
        gates = GatherGatesAndSave();
        SetupGateColors();
        source = gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {

    }


    #region Public Methods
    //temp summary
    // gate actions depend on the gates that use them, for control flow across the timer system
    public void StartGate(GameObject completedGate)
    {
        IterateGateLogic(completedGate);
        //Debug.Log("starting on the handler");
        StartGateEvent.Invoke(true);
    }

    public void IterateGate(GameObject completedGate)
    {
        IterateGateLogic(completedGate);
        //Debug.Log("iterating on the handler");
        IterateGateEvent.Invoke();
    }

    public void EndGate(GameObject completedGate)
    {
        EndGateEvent.Invoke();
        IterateGateLogic(completedGate);
        //Debug.Log("end on the handler");
    }

    /// <summary>
    /// Calculates the percentage of gates that have been completed.
    /// </summary>
    /// <returns>A float value representing the percentage of gates that have been completed.</returns>
    public float CalcGateCompletion()
    {
        Debug.Log(completedGateAmount);
        Debug.Log(gates.Count());
        float clampedValue = (gates.Count() - 1) / completedGateAmount;
        Debug.Log(clampedValue);
        return clampedValue;
    }

    public TimeRank DetermineScore()
    {
        float gateCompletion = CalcGateCompletion();
        float mapTime = gameObject.GetComponent<GateTimer>().courseTime;

        //see how much over or under the players time is
        float timeAfterReductionOfConst = (mapTime * gateCompletion) - StandardTime;
        Debug.Log(timeAfterReductionOfConst);
        //determine the point count against the time
        int pointValue = (int)(timeAfterReductionOfConst / 5f);
        Debug.Log(pointValue);

        // Handle the case where the timeAfterReductionOfConst variable is negative.
        if (timeAfterReductionOfConst < 0)
        {
            return TimeRank.S;
        }

        // Determine the rank based on the point value.
        switch (pointValue)
        {
            case 0:
                return TimeRank.A;
            case 1:
                return TimeRank.B;
            case 2:
                return TimeRank.C;
            case 3:
                return TimeRank.D;
            case -1: // Add a new case to handle the negative point value
                return TimeRank.S;
            default:
                return TimeRank.F;
        }
    }
    #endregion

    #region Private methods
    
    //i need to call this after the gate setup
    private void SetupGateColors()
    {
        for (int i = 0; i < gates.Count(); i++)
        {
            GameObject gateObject = gates[i].gameObject;
            // Get the child transform of the parent object
            Transform canvasObject = gateObject.transform.GetChild(0);

            // Get the grandchild transform of the parent object
            Transform ImageObject = canvasObject.GetChild(0);
            Image gateImage = ImageObject.GetComponent<Image>();

            //set the gates new color to the value assigned through the gradient
            SetGateColor(gateImage, i);
        }
    }

    /// <summary>
    /// Iterates over the gates list and updates their states and colors.
    /// </summary>
    /// <param name="completedGate">The gate that was recently completed.</param>
    private void IterateGateLogic(GameObject completedGate)
    {
        //find gate activated
        bool doesMatch = gates.Contains(completedGate);
        int foundIndex = 0;

        if (doesMatch)
        {
            //find the index of the object
            int index = -1;

            for (int i = 0; i <= gates.Count(); i++)
            {
                if (GameObject.ReferenceEquals(gates[i], completedGate))
                {
                    foundIndex = i;
                    index = foundIndex;
                    break;
                }
            }

            if (index != -1)
            {
                // The GameObject is in the list at index `index`.
                foundIndex = index;
            }
            else
            {
                //else, the gameobject was not in the array at a viable index value
                Debug.LogWarning($"Object {completedGate} does not have an index in the gates array or was out of bounds");
            }
        }
        else
        {
            //warn the developer and return.
            Debug.LogWarning($"Object {completedGate} not found in the gates array");
            return;
        }

        //bump up the current gate index, and set previous gates to disabled
        completedGateAmount++;
        gates[foundIndex].gameObject.SetActive(false);

        //it will talley the missed gates, and save them so we can refer back to if they missed a gate or not
        for (int i = 0; i <= foundIndex; i++)
        {
            if (gates[i].gameObject.activeInHierarchy)
            {
                gates[i].gameObject.SetActive(false);
                missedGateAmount++;
            }
        }

        SetActiveGateColors(foundIndex);
    }

    private void SetActiveGateColors(int foundIndex)
    {
        // set the colors for the currently still active gates
        for (int i = foundIndex; i < gates.Count(); i++)
        {

            GameObject gateObject = gates[i].gameObject;
            // Get the child transform of the parent object
            Transform canvasObject = gateObject.transform.GetChild(0);

            // Get the grandchild transform of the parent object
            Transform ImageObject = canvasObject.GetChild(0);
            Image gateImage = ImageObject.GetComponent<Image>();

            //set the gates new color to the value assigned through the gradient
            SetGateColor(gateImage, i - foundIndex);
        }
    }

    /// <summary>
    /// Sets the color of the gate image based on its local index.
    /// </summary>
    /// <param name="image">The gate image.</param>
    /// <param name="localIndex">The local index of the gate image.</param>
    private void SetGateColor(Image image, int localIndex)
    {
        int gateCount = gates.Count() - 1;
        float gateGradientColor = (float)localIndex / (float)gateCount;

        image.color = gateGradient.Evaluate(gateGradientColor);
    }

    public void PlayGateSound(int i)
    {
        source.PlayOneShot(clip[i]);
    }

    [BurstCompile]
    private GameObject[] GatherGatesAndSave()
    {
        //find the gate container
        GameObject gateContainer = GameObject.FindWithTag("GateContainer");
        //get all the xchildren, in the order thewy are under the collider
        if(gateContainer != null)
        {
            GameObject[] children = new GameObject[gateContainer.transform.childCount];
            for (int i = 0; i < gateContainer.transform.childCount; i++)
            {
                children[i] = gateContainer.transform.GetChild(i).gameObject;
            }
            //after gathering the children into the array return the array. 
            return children;
        }
        else
        {
            //
            Debug.LogError("Children of gate container not found, failed to add to array.");
            return null;
        }
    }


    #endregion


}

public enum TimeRank
{
    S,
    A,
    B,
    C,
    D,
    F
}
