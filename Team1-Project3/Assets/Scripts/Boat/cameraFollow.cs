using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    [SerializeField] private Transform targetPosition;
    [SerializeField] private Transform targetToTrack;
    [SerializeField] private float smoothTime = 0.3f;
    private Vector3 targetVelocity = Vector3.zero;
    private Vector3 trackingVelocity = Vector3.zero;

    void FixedUpdate()
    {
        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition.position, ref targetVelocity, smoothTime);
        gameObject.transform.LookAt(Vector3.SmoothDamp(transform.position, targetToTrack.position, ref trackingVelocity, smoothTime));
    }
}
