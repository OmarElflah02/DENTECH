using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class voxelScript : MonoBehaviour
{
    public List<Vector3Int> GridPoints = new List<Vector3Int>();
    public float HalfSize = 0.1f; // Half the size of a voxel
    public Vector3 LocalOrigin;

    public Vector3 PointToPosition(Vector3Int point)
    {
        return LocalOrigin + new Vector3(point.x, point.y, point.z) * (HalfSize * 2);
    }

    public static void VoxelizeMesh(MeshFilter meshFilter)
    {
        if (!meshFilter.TryGetComponent(out MeshCollider meshCollider))
        {
            meshCollider = meshFilter.gameObject.AddComponent<MeshCollider>();
        }

        if (!meshFilter.TryGetComponent(out voxelScript voxelizedMesh))
        {
            voxelizedMesh = meshFilter.gameObject.AddComponent<voxelScript>();
        }

        Bounds bounds = meshCollider.bounds;
        Vector3 minExtents = bounds.center - bounds.extents;
        float halfSize = voxelizedMesh.HalfSize;
        Vector3 count = bounds.extents / halfSize;

        int xMax = Mathf.CeilToInt(count.x);
        int yMax = Mathf.CeilToInt(count.y);
        int zMax = Mathf.CeilToInt(count.z);

        voxelizedMesh.GridPoints.Clear();
        voxelizedMesh.LocalOrigin = voxelizedMesh.transform.InverseTransformPoint(minExtents);

        for (int x = 0; x < xMax; ++x)
        {
            for (int z = 0; z < zMax; ++z)
            {
                for (int y = 0; y < yMax; ++y)
                {
                    Vector3 pos = voxelizedMesh.PointToPosition(new Vector3Int(x, y, z));
                    if (Physics.CheckBox(pos, new Vector3(halfSize, halfSize, halfSize)))
                    {
                        voxelizedMesh.GridPoints.Add(new Vector3Int(x, y, z));
                    }
                }
            }
        }
    }
    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            VoxelizeMesh(meshFilter);
        }
    }
}
