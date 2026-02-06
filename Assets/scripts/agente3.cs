using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class agente3 : MonoBehaviour
{
    public NavMeshAgent agent;

    [Header("Terrain Speeds")]
    public float grassspeed = 5f;
    public float sandspeed = 4f;
    public float mudspeed = 2f;

    [Header("Mouse")]
    public LayerMask groundLayer;
    public float navMeshSampleDistance = 2f;

    Camera cam;

    int grassarea;
    int sandarea;
    int mudarea;

    void Awake()
    {
        cam = Camera.main;
        grassarea = NavMesh.GetAreaFromName("grass");
        sandarea = NavMesh.GetAreaFromName("sand");
        mudarea = NavMesh.GetAreaFromName("mud");

        Debug.Log($"Areas grass:{grassarea} sand:{sandarea} mud:{mudarea}");
    }

    void Update()
    {
        MoveAgentToMouse();
        UpdateSpeedBasedOnTerrain();
        Debug.Log(agent.speed);
        /*if (NavMesh.SamplePosition(transform.position, out var hit, 10f, NavMesh.AllAreas))
        {
            agent.speed = 1f + hit.mask * 0.01f;
            Debug.Log("HIT " + hit.mask);
        }*/

    }

    void MoveAgentToMouse()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();

        Ray ray = cam.ScreenPointToRay(mouseScreenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundLayer))
        {
            if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, navMeshSampleDistance, NavMesh.AllAreas))
            {
                agent.SetDestination(navHit.position);
            }
        }
    }

    void UpdateSpeedBasedOnTerrain()
    {
        if (NavMesh.SamplePosition(transform.position, out var hit, agent.height, NavMesh.AllAreas))
        {
            int areaIndex = GetAreaIndexFromMask(hit.mask);

            float targetSpeed = agent.speed;

            if (areaIndex == grassarea)
                targetSpeed = grassspeed;
            else if (areaIndex == sandarea)
                targetSpeed = sandspeed;
            else if (areaIndex == mudarea)
                targetSpeed = mudspeed;

            agent.speed = targetSpeed;

            Debug.Log($"AreaIndex: {areaIndex} | Speed: {agent.speed}");
        }
    }

    int GetAreaIndexFromMask(int mask)
    {
        for (int i = 0; i < 32; i++)
        {
            if ((mask & (1 << i)) != 0)
                return i;
        }
        return -1;
    }

}