using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartGateInteractor : GateInteractor
{
    private void StartGate(Collider other)
    {
        //get the handler, and then pass its own gameobject as the paramter.
        GateHandler handler = FindHandler(other);
        handler.StartGate(gameObject);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject);
        StartGate(other);
    }
}
