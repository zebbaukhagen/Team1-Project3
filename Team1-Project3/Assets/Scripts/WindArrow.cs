using UnityEngine;
using UnityEngine.UI;

public class WindArrow : MonoBehaviour
{
    private RectTransform rectTransform;
    private WindManager manager;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        RotateArrow();
    }

    private void RotateArrow()
    {
        // The default direction of the arrow (assuming it points upwards when not rotated).
        Vector3 defaultDirection = Vector3.right;

        // Calculate the angle between the wind direction and the default arrow direction.
        float angle = Vector3.SignedAngle(defaultDirection, manager.m_currentWindDirection, Vector3.forward);

        // Set the rotation of the arrow's rectTransform.
        rectTransform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
