using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class ClickToMoveWithTerrain : MonoBehaviour
{
    [Header("References")]
    public Camera cam;                   // Your main camera
    public NavMeshAgent agent;           // The NavMeshAgent
    public LayerMask groundMask;         // Layer for terrain clicks

    [Header("Speeds")]
    public float grassSpeed = 3f;
    public float sandSpeed = 2f;
    public float mudSpeed = 1f;

    [Header("Animation")]
    public Animator animator;

    void Update()
    {
        HandleClickMove();
        UpdateSpeed();
    }

    void HandleClickMove()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundMask))
            {
                // Sample NavMesh at the clicked point
                if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 5f, NavMesh.AllAreas))
                {
                    // Determine terrain **before moving** so speed is correct
                    SetSpeedBasedOnTerrain();

                    // Move agent
                    agent.SetDestination(navHit.position);
                }
            }
        }
    }

    void UpdateSpeed()
    {
        // Continuously update speed as agent moves over different terrain
        SetSpeedBasedOnTerrain();
    }

    void SetSpeedBasedOnTerrain()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(agent.transform.position, out hit, 5f, NavMesh.AllAreas))
        {
            if (IsOnArea(hit, "mud"))
            {
                agent.speed = mudSpeed;
                animator.SetInteger("ground", 2);
            }
            else if (IsOnArea(hit, "sand"))
            {
                agent.speed = sandSpeed;
                animator.SetInteger("ground", 1);

            }
            else
            {
                agent.speed = grassSpeed;
                animator.SetInteger("ground", 0);
            }
        }
    }

    bool IsOnArea(NavMeshHit hit, string areaName)
    {
        int areaIndex = NavMesh.GetAreaFromName(areaName);
        int areaMask = 1 << areaIndex;
        return (hit.mask & areaMask) != 0;
    }
}
