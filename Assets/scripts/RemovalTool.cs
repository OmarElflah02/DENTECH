// using UnityEngine;
// using System.Collections.Generic;

// [RequireComponent(typeof(MeshFilter))]
// public class MeshModificationArea : MonoBehaviour
// {
//     public GameObject tool; // Assign the tool GameObject in the Inspector
//     public float removalRadius = 1f; // Radius within which vertices will be removed

//     private void OnTriggerStay(Collider other)
//     {
//         if (other.gameObject == tool)
//         {
//             Vector3 rayDirection = transform.position - tool.transform.position;

//             if (Physics.Raycast(tool.transform.position, rayDirection, out RaycastHit hit, Mathf.Infinity))
//             {
//                 Debug.Log("Raycast hit at " + hit.point);
//                 Mesh mesh = GetComponent<MeshFilter>().mesh;
//                 RemoveVerticesInRadius(hit.point, mesh, removalRadius);
//                 UpdateMeshCollider();
//             }
//             else
//             {
//                 Debug.Log("Raycast did not hit the mesh.");
//             }
//         }
//     }

//     void RemoveVerticesInRadius(Vector3 point, Mesh mesh, float radius)
//     {
//         Vector3[] vertices = mesh.vertices;
//         List<int> triangles = new List<int>(mesh.triangles);
//         Transform meshTransform = GetComponent<Transform>();

//         // Track vertices to remove
//         HashSet<int> verticesToRemove = new HashSet<int>();

//         for (int i = 0; i < vertices.Length; i++)
//         {
//             if (Vector3.Distance(meshTransform.TransformPoint(vertices[i]), point) <= radius)
//             {
//                 verticesToRemove.Add(i);
//             }
//         }

//         // Create a list to hold the new triangles
//         List<int> newTriangles = new List<int>();

//         // Iterate through the old triangles, skipping those that contain any vertex to remove
//         for (int i = 0; i < triangles.Count; i += 3)
//         {
//             if (!verticesToRemove.Contains(triangles[i]) && !verticesToRemove.Contains(triangles[i + 1]) && !verticesToRemove.Contains(triangles[i + 2]))
//             {
//                 // This triangle does not contain any vertex to remove, so add it to the new list
//                 newTriangles.Add(triangles[i]);
//                 newTriangles.Add(triangles[i + 1]);
//                 newTriangles.Add(triangles[i + 2]);
//             }
//             else{
//                 Debug.Log("Triangle removed due to vertex removal." + i + " " + (i + 1) + " " + (i + 2) );
//             }
//         }

//         // Update the mesh with the new triangles
//         mesh.triangles = newTriangles.ToArray();
//         mesh.RecalculateBounds();
//         mesh.RecalculateNormals();

//         Debug.Log($"Mesh updated. Vertex count: {mesh.vertexCount}. Removed vertices: {verticesToRemove.Count}");
//     }

//     void UpdateMeshCollider()
//     {
//         MeshCollider meshCollider = GetComponent<MeshCollider>();
//         if (meshCollider != null)
//         {
//             meshCollider.sharedMesh = null;
//             meshCollider.sharedMesh = GetComponent<MeshFilter>().mesh;
//             Debug.Log("Mesh collider updated.");
//         }
//     }
// }

using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
public class MeshModificationProximity : MonoBehaviour
{
    HashSet<int> verticesWasRemoved = new HashSet<int>();
    public GameObject tool; // Assign the tool GameObject in the Inspector
    public float removalRadius = 1f; // Radius within which vertices will be removed

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == tool)
        {
            // Use the tool's position as the point of impact
            // Vector3 toolPosition = other.ClosestPointOnBounds(transform.position); // Get the closest point on the tool's collider to this mesh
            Vector3 toolPosition = other.transform.position;
            RemoveVerticesInRadius(toolPosition, removalRadius);
            UpdateMeshCollider();
        }
    }

    void RemoveVerticesInRadius(Vector3 point, float radius)
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        Transform meshTransform = GetComponent<Transform>();

        HashSet<int> verticesToRemove = new HashSet<int>();

        // Convert point to local space for comparison
        
        // Debug.Log("Tool " + point);
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 localPoint = meshTransform.TransformPoint(vertices[i]);
            // if(i == 0){
            //     Debug.Log("Vertex localPoint" + i + " " + localPoint);
            //     Debug.Log("Vertex localPoint Vector3.Distance(localPoint, point)" + i + " " + Vector3.Distance(localPoint, point) + " " + radius + " " + (Vector3.Distance(localPoint, point) <= radius)    );
            // }
            if (Vector3.Distance(localPoint, point) <= 0.35 && Vector3.Distance(localPoint, point) >= 0.3499  && !verticesWasRemoved.Contains(i))
            // if (i > 1500 && i < 2100 && !verticesWasRemoved.Contains(i))
            {
                verticesWasRemoved.Add(i);
                verticesToRemove.Add(i);
                // Debug.Log("Vertex removed due to proximity: " + i + " " + meshTransform.TransformPoint(vertices[i]) + " " + point + " " + radius + " " + Vector3.Distance(meshTransform.TransformPoint(vertices[i]), point));
            }
            // I want to randomly remove vertices within the radius
            // if (Random.Range(0, 8000) < 2)
            // {
            //     verticesToRemove.Add(i);
            // }
        }

        List<int> newTriangles = new List<int>();

        for (int i = 0; i < triangles.Length; i += 3)
        {
            if (!verticesToRemove.Contains(triangles[i]) && !verticesToRemove.Contains(triangles[i + 1]) && !verticesToRemove.Contains(triangles[i + 2]))
            {
                newTriangles.Add(triangles[i]);
                newTriangles.Add(triangles[i + 1]);
                newTriangles.Add(triangles[i + 2]);
            }
        }

        mesh.triangles = newTriangles.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        // Debug.Log($"Mesh updated. Vertex count: {mesh.vertexCount}. Removed vertices: {verticesToRemove.Count}");
    }

    void UpdateMeshCollider()
    {
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            meshCollider.sharedMesh = null;
            meshCollider.sharedMesh = GetComponent<MeshFilter>().mesh;
            // Debug.Log("Mesh collider updated.");
        }
    }
}
