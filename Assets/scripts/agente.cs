using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class agente : MonoBehaviour
{
    public NavMeshAgent agent;
    public List<Transform> destinations = new List<Transform>();

    void Start()
    {
        GoToRandomDestination();
    }

    void Update()
    {
        if (agent.pathPending)
            return;

        // Check if agent reached the destination
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                GoToRandomDestination();
            }
        }
    }

    private int lastIndex = -1;

    void GoToRandomDestination()
    {
        if (destinations.Count == 0)
            return;

        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, destinations.Count);
        }
        while (randomIndex == lastIndex && destinations.Count > 1);

        lastIndex = randomIndex;
        agent.SetDestination(destinations[randomIndex].position);
    }
}