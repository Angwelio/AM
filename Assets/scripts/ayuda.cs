using UnityEngine;
using UnityEngine.AI;

public class ayuda : MonoBehaviour
{
    void Start()
    {
        Debug.Log("NavMesh triangulation verts: " + NavMesh.CalculateTriangulation().vertices.Length);
    }
}

