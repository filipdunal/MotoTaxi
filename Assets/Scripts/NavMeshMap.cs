using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshMap : MonoBehaviour
{
    NavMeshTriangulation triangles;
    Mesh mesh;
    MeshFilter navMeshMap;
    
    public void RenderMap()
    {
        navMeshMap = GetComponent<MeshFilter>();
        triangles = NavMesh.CalculateTriangulation();
        mesh = new Mesh();
        mesh.vertices = triangles.vertices;
        mesh.triangles = triangles.indices;
        navMeshMap.mesh = mesh;
    }

    public void EraseMap()
    {
        GetComponent<MeshFilter>().mesh = null;
    }
}
