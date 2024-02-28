using UnityEngine;
using System.Collections; // Required for IEnumerator

public class ToolInteraction : MonoBehaviour
{
    private bool isProcessing = false; // Flag to control coroutine execution

    private void OnTriggerStay(Collider other)
    {
        // Check if the collided object is tagged as "tooth" and ensure we're not already processing
        if (other.gameObject.CompareTag("tooth"))
        {
            PerformBooleanDifference(other.gameObject);
        }
    }




    private void PerformBooleanDifference(GameObject tooth)
    {
        // Your existing implementation
        Model resultModel = CSG.Perform(CSG.BooleanOp.Subtraction, tooth, this.gameObject);
        if (resultModel != null && resultModel.mesh != null)
        {
            var composite = new GameObject();
            composite.AddComponent<MeshFilter>().sharedMesh = resultModel.mesh;
            resultModel.materials.Add(tooth.GetComponent<MeshRenderer>().sharedMaterial);
            composite.AddComponent<MeshRenderer>().sharedMaterials = resultModel.materials.ToArray();
            MeshCollider meshCollider = tooth.GetComponent<MeshCollider>();
            MeshRenderer meshRenderer = tooth.GetComponent<MeshRenderer>();
            meshCollider.sharedMesh = null; // Clear current mesh to ensure update
            meshCollider.sharedMesh = resultModel.mesh; // Assign the new mesh for physics
            meshRenderer.materials = resultModel.materials.ToArray();


        }
        else
        {
            Debug.LogError("Boolean subtraction failed or resulted in an invalid mesh.");
        }

    }

    // private void UpdateMeshCollider(GameObject tooth, Mesh newMesh)
    // {
    //     MeshCollider meshCollider = tooth.GetComponent<MeshCollider>();
    //     if (meshCollider != null)
    //     {
    //         meshCollider.sharedMesh = null; // Clear current mesh to ensure update
    //         meshCollider.sharedMesh = newMesh; // Assign the new mesh for physics
    //     }
    //     else
    //     {
    //         Debug.LogError("MeshCollider component not found on the tooth object!");
    //     }
    // }
}
