using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManager : MonoBehaviour
{
    [Header("Accessible Variables")]
    public Vector3 m_currentWindDirection;
    public float m_currentWindForce;

    [Header("Global Direction and Speed")]
    [SerializeField] private Vector3 m_globalWindDirection = Vector3.forward;
    [SerializeField] private float m_globalWindForce = 10f;

    [Header("Wind Collider Data")]
    public bool m_useCustomVector = false;
    [SerializeField] private List<WindColliderData> m_colliders;
    private WindColliderData m_currentWindColliderData;


    private void Start()
    {
        m_currentWindDirection = m_globalWindDirection;
        m_currentWindForce = m_globalWindForce;
    }


    /// <summary>
    /// Sets the current wind collider data to the associated struct in the inspector
    /// </summary>
    /// <param name="windObject">the object that calls the method</param>
    public void SetWindIndex(GameObject windObject)
    {
        foreach(WindColliderData data in m_colliders)
        {
            //find the linked object in the list and save it to our custom data variable
            if(windObject.GetInstanceID() == data.colliderOBJ.GetInstanceID())
            {
                m_useCustomVector = true;
                m_currentWindColliderData = data;
                DetermineWindVector();
                //end the loop immediately
                return;
            }
        }
        Debug.LogWarning($"{windObject} was not found in scope of the listed colliders");
    }

    /// <summary>
    /// Releases the wind manager from using the custom wind data
    /// </summary>
    public void ReleaseFromWindCollider()
    {
        m_useCustomVector = false;
        DetermineWindVector();
    }

    /// <summary>
    /// Determines the use of the global vs selected local wind data
    /// </summary>
    private void DetermineWindVector()
    {
        if (!m_useCustomVector)
        {
            //set our currst accessible variables to our global corresponding variables
            m_currentWindDirection = m_globalWindDirection;
            m_currentWindForce = m_globalWindForce;
        }
        else
        {
            //use the object itself as a direction and grab our stored speed in the inspector
            m_currentWindDirection = m_currentWindColliderData.colliderOBJ.transform.forward;
            Debug.Log(m_currentWindDirection);
            m_currentWindForce = m_currentWindColliderData.localWindSpeed;
        }
    }

    /// <summary>
    /// the storage medium for comparison of each collider
    /// </summary>
    [System.Serializable]
    public struct WindColliderData
    {
        [SerializeField] public GameObject colliderOBJ;
        [SerializeField] public float localWindSpeed;
    }
}


