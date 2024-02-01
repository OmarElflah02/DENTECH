using System.Collections.Generic;
using UnityEngine;

public class GazeDetector : MonoBehaviour
{
    public float gazeDistance = 100f; // Distance within which to detect objects
    public LayerMask detectableLayers; // Only detect objects on these layers
    public List<Transform> objectsToAttach = new List<Transform>(); // List of objects to attach to the camera
    public Vector3 attachmentOffset; // Offset from the camera's position

    private Transform currentObjectGazedAt = null; // Currently gazed object

    void Update()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        bool hitDetected = Physics.Raycast(transform.position, fwd, out hit, gazeDistance, detectableLayers);
        if (currentObjectGazedAt == null && hitDetected)
        {
            Transform hitTransform = hit.transform;
            // Check if the hit object is one of the objects to attach
            if (objectsToAttach.Contains(hitTransform))
            {
                Debug.Log("Hit objectToAttach");
                currentObjectGazedAt = hitTransform;
                AttachObjectToCamera(hitTransform);
            }
        }
        else if (currentObjectGazedAt != null )
        {
            // If no object is hit and there was a previously attached object, detach it
            // Optional: Implement logic to "detach" if needed
            AttachObjectToCamera(currentObjectGazedAt);
        }
        // else if (currentObjectGazedAt != null)
        // {
        //     // If no object is hit and there was a previously attached object, detach it
        //     // Optional: Implement logic to "detach" if needed
        //     currentObjectGazedAt = null;
        // }
    }

    void AttachObjectToCamera(Transform objectToAttach)
    {
        // Adjust the object's position and rotation to match the camera's, applying the specified offset
        objectToAttach.position = Camera.main.transform.position + Camera.main.transform.TransformDirection(attachmentOffset);
        objectToAttach.rotation = Camera.main.transform.rotation;
    }
}
