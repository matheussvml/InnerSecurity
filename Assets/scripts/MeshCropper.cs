using System.Collections.Generic;
using UnityEngine;

public class MeshCropper : MonoBehaviour
{
    public Vector3 minBounds; // canto mínimo da região (x, y, z)
    public Vector3 maxBounds; // canto máximo da região (x, y, z)

    public void CropMesh()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        if (mf == null)
        {
            Debug.LogError("Não tem MeshFilter nesse objeto!");
            return;
        }

        Mesh originalMesh = mf.mesh;
        Vector3[] vertices = originalMesh.vertices;
        int[] triangles = originalMesh.triangles;

        List<Vector3> newVertices = new List<Vector3>();
        List<int> newTriangles = new List<int>();
        Dictionary<int, int> vertexMap = new Dictionary<int, int>();

        for (int i = 0; i < triangles.Length; i += 3)
        {
            int i0 = triangles[i];
            int i1 = triangles[i + 1];
            int i2 = triangles[i + 2];

            Vector3 v0 = vertices[i0];
            Vector3 v1 = vertices[i1];
            Vector3 v2 = vertices[i2];

            // Verifica se todos os vértices do triângulo estão dentro da região
            if (IsInside(v0) && IsInside(v1) && IsInside(v2))
            {
                int[] newIndices = new int[3];
                int[] oldIndices = new int[] { i0, i1, i2 };

                for (int j = 0; j < 3; j++)
                {
                    int oldIndex = oldIndices[j];
                    if (!vertexMap.ContainsKey(oldIndex))
                    {
                        vertexMap[oldIndex] = newVertices.Count;
                        newVertices.Add(vertices[oldIndex]);
                    }
                    newIndices[j] = vertexMap[oldIndex];
                }

                newTriangles.AddRange(newIndices);
            }
        }

        Mesh newMesh = new Mesh();
        newMesh.vertices = newVertices.ToArray();
        newMesh.triangles = newTriangles.ToArray();
        newMesh.RecalculateNormals();
        newMesh.RecalculateBounds();

        GameObject newObj = new GameObject("CroppedMesh");
        newObj.transform.position = transform.position;
        newObj.transform.rotation = transform.rotation;

        MeshFilter newMF = newObj.AddComponent<MeshFilter>();
        newMF.mesh = newMesh;

        MeshRenderer newMR = newObj.AddComponent<MeshRenderer>();
        newMR.material = GetComponent<MeshRenderer>().material;

        MeshCollider newMC = newObj.AddComponent<MeshCollider>();
        newMC.sharedMesh = newMesh;

        Debug.Log("Novo mesh criado com " + newVertices.Count + " vértices e " + (newTriangles.Count / 3) + " triângulos.");
    }

    private bool IsInside(Vector3 v)
    {
        return v.x >= minBounds.x && v.x <= maxBounds.x &&
               v.y >= minBounds.y && v.y <= maxBounds.y &&
               v.z >= minBounds.z && v.z <= maxBounds.z;
    }
}
