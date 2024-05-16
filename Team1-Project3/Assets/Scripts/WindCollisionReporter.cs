using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindCollisionReporter : MonoBehaviour
{
    WindManager windManager;
    public void AssignManager()
    {
        windManager = transform.parent.GetComponent<WindManager>();
    }

    private void Start()
    {
        AssignManager();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (windManager != null)
        {
            windManager.SetWindIndex(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (windManager != null)
        {
            windManager.ReleaseFromWindCollider();
        }
    }
}
