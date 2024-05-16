using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EndGateInteractor : GateInteractor
{
    public PlayerUI uiHARDLINE;


    private void EndGate(Collider other)
    {
        //get the handler, and then pass its own gameobject as the paramter.
        GateHandler handler = FindHandler(other);
        handler.EndGate(gameObject);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        uiHARDLINE.EndLevel();
        EndGate(other);
    }
}
