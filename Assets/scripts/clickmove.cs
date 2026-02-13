using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class clickmove : MonoBehaviour
{
    [Header("References")]
    public Camera cam;
    public NavMeshAgent agent;
    public LayerMask groundMask;

    PlayerInputActions input;

    void Awake()
    {
        input = new PlayerInputActions();
    }

    void OnEnable()
    {
        input.Enable();
        input.Player.click.performed += OnClick;
    }

    void OnDisable()
    {
        input.Player.click.performed -= OnClick;
        input.Disable();
    }

    void OnClick(InputAction.CallbackContext ctx)
    {
        Debug.Log("CLICK REGISTERED");
        if (Mouse.current == null)
            return;

        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, 1000f, groundMask))
        {
            Debug.Log("RAYCAST MISSED");
            return;
        }
        Debug.Log("RAYCAST HIT: " + hit.collider.name);

        NavMeshHit navHit;
        if (NavMesh.SamplePosition(hit.point, out navHit, 1.0f, NavMesh.AllAreas))
        {
            agent.SetDestination(navHit.position);
        }
    }
}
