using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GateTimer : MonoBehaviour
{
    public float courseTime = 0;
    public List<float> gateTimes = new List<float>();

    private bool courseIsRunning = false;

    

    private void Update()
    {
        if (courseIsRunning)
        {
            courseTime = courseTime + Time.deltaTime;
        }
    }

    public void HandleCourseTimer(bool state)
    {
        //freely pause or start the course timer with a simple method
        courseIsRunning = state;
        Debug.Log($"setting the timer state to {state}");
    }
    public void HandleCourseTimer()
    {
        Debug.Log("stoppingthe timer");
        //just toggle, dont set state
        courseIsRunning = !courseIsRunning;
    }

    public void SaveGateTime()
    {
        Debug.Log("saving new gate time");
        // Declare a variable to store the new gate time.
        float newGateTime;

        // Check if the list of gate times is empty.
        if (gateTimes.Count <= 0)
        {
            // If the list is empty, set the new gate time to the current course time.
            newGateTime = courseTime;
        }
        else
        {
            // If the list is not empty, calculate the new gate time by subtracting the current course time from the last saved course time.
            newGateTime = courseTime - gateTimes[gateTimes.Count - 1];
        }

        // Add the new gate time to the list of gate times.
        gateTimes.Add(newGateTime);
    }
}


