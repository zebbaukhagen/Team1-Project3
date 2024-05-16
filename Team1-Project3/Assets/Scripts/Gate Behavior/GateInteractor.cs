using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GateInteractor : MonoBehaviour
{
    public UnityEvent OnIterateGateEvent = new UnityEvent();
    private void IterateGate(Collider other)
    {
        //get the handler, and then pass its own gameobject as the paramter.
        GateHandler handler = FindHandler(other);
        handler.IterateGate(gameObject);
        handler.PlayGateSound(1); // play the second activate sound
    }

    protected GateHandler FindHandler(Collider other)
    {
        if (!(other.gameObject.layer == 6))
        {
            return null;
        }
        GateHandler boatGateHandler = other.GetComponent<GateHandler>();
        if (boatGateHandler != null)
        {

            return boatGateHandler;
        }
        return null;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        IterateGate(other);
    }

    
}
