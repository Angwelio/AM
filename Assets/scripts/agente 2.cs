using UnityEngine;
using UnityEngine.AI;

public class agente2 : MonoBehaviour
{
    public NavMeshAgent agent;
    public float wanderRadius = 50f;
    public float waitTime = 1.5f;

    private float timer;

    void Start()
    {
        timer = waitTime;
        SetRandomDestination();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (timer >= waitTime)
            {
                SetRandomDestination();
                timer = 0f;
            }
        }
    }

    void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}
