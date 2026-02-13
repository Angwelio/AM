using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class agente3 : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;

    [Header("Speeds")]
    public float grassSpeed = 3.5f;
    public float sandSpeed = 2.5f;
    public float mudSpeed = 1.8f;

    int grassArea;
    int sandArea;
    int mudArea;

    void Start()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 5f, NavMesh.AllAreas))
        {
            GetComponent<NavMeshAgent>().Warp(hit.position);
        }
    }

    void Awake()
    {
        grassArea = NavMesh.GetAreaFromName("grass");
        sandArea = NavMesh.GetAreaFromName("sand");
        mudArea = NavMesh.GetAreaFromName("mud");
    }

    void Update()
    {
        Debug.Log(agent.speed);
        UpdateSpeedByArea();
    }

    void UpdateSpeedByArea()
    {
        NavMeshHit hit;

        if (!NavMesh.SamplePosition(transform.position, out hit, 3f, NavMesh.AllAreas))
            return;
        Debug.Log("Area mask: " + hit.mask);

        int areaMask = hit.mask;

        if (IsOnArea(areaMask, mudArea))
            agent.speed = mudSpeed;
        else if (IsOnArea(areaMask, sandArea))
            agent.speed = sandSpeed;
        else
            agent.speed = grassSpeed;
    }

    bool IsOnArea(int mask, int areaIndex)
    {
        return (mask & (1 << areaIndex)) != 0;
    }
}