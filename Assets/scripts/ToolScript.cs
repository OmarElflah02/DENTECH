using UnityEngine;
using System.Collections.Generic; // This might be needed depending on how the rest of the CSG library is structured
using static CSG; // Ensure this matches the actual namespace and class of your CSG library

public class ToolInteraction : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        // Check if the collided object is tagged as "tooth"
        if (other.gameObject.CompareTag("tooth"))
        {
            PerformBooleanDifference(other.gameObject);
        }
    }

    private Mesh ModelToMesh(Model model)
    {
        Mesh mesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        // Add code here to populate vertices and triangles from the model's data

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.RecalculateNormals(); // Recalculate normals for proper lighting
        return mesh;
    }

    private void PerformBooleanDifference(GameObject tooth)
    {
        // Assuming 'Perform' is a static method in your CSG library that performs the boolean operation
        // and 'this.gameObject' is correctly set up for the operation
        Model resultModel = CSG.Perform(CSG.BooleanOp.Subtraction, tooth, this.gameObject);

        if (resultModel != null)
        {
            // Convert the CSG Model result into a Unity Mesh
            Mesh resultMesh = ModelToMesh(resultModel);

            // Apply the resulting mesh to the tooth
            tooth.GetComponent<MeshFilter>().mesh = resultMesh;
            UpdateMeshCollider(tooth, resultMesh);
        }
        else
        {
            Debug.LogError("Boolean subtraction failed.");
        }
    }

    private void UpdateMeshCollider(GameObject tooth, Mesh newMesh)
    {
        MeshCollider meshCollider = tooth.GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            meshCollider.sharedMesh = null; // Clear current mesh
            meshCollider.sharedMesh = newMesh; // Assign the new mesh
        }
        else
        {
            Debug.LogError("MeshCollider component not found on the tooth object!");
        }
    }
}
