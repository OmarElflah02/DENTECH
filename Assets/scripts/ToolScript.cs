using UnityEngine;
using System.Collections; // Required for IEnumerator
using UnityEngine;
using System.Collections;

public class ToolInteraction : MonoBehaviour
{
    private bool isProcessing = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("tooth") && !isProcessing)
        {
            StartCoroutine(PerformWithInterval(other.gameObject));
        }
    }

    private IEnumerator PerformWithInterval(GameObject tooth)
    {
        isProcessing = true;
        Debug.Log("Collided with " + tooth.name);
        PerformBooleanDifference(tooth);
        yield return new WaitForSeconds(2f); // Wait for 2 seconds
        isProcessing = false;
    }

    private void PerformBooleanDifference(GameObject tooth)
    {
        Debug.Log("Performing boolean difference operation...");
        // Your existing implementation
        Model resultModel = CSG.Perform(CSG.BooleanOp.Subtraction, tooth, this.gameObject);
        if (resultModel != null && resultModel.mesh != null)
        {
            // Update the mesh of the tooth object
            UpdateMeshCollider(tooth, resultModel.mesh);

        }
        else
        {
            Debug.LogError("Boolean subtraction failed or resulted in an invalid mesh.");
        }

    }

    private void UpdateMeshCollider(GameObject tooth, Mesh newMesh)
    {
        MeshCollider meshCollider = tooth.GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            meshCollider.sharedMesh = null; // Clear current mesh to ensure update
            meshCollider.sharedMesh = newMesh; // Assign the new mesh for physics
        }
        else
        {
            Debug.LogError("MeshCollider component not found on the tooth object!");
        }
    }
}
