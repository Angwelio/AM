
using Unity.Netcode;
using Unity.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerNetwork : NetworkBehaviour
{
    [Header("Visual References")]
    [SerializeField] private Renderer playerRenderer;
    [SerializeField] private TextMeshPro nameText;
    private PlayerInputActions playerinput;

    // Nombre sincronizado (solo el servidor puede escribir)
    public NetworkVariable<FixedString32Bytes> playerName =
        new NetworkVariable<FixedString32Bytes>(
            default,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server);

    // Color sincronizado (solo el servidor puede escribir)
    public NetworkVariable<Color> playerColor =
        new NetworkVariable<Color>(
            default,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server);

    private void Awake()
    {
        if (playerRenderer == null)
            playerRenderer = GetComponentInChildren<Renderer>();
            playerinput = new PlayerInputActions();
    }

    public override void OnNetworkSpawn()
    {
        // Solo el servidor asigna valores iniciales
        if (IsServer)
        {
            playerName.Value = "Player_" + OwnerClientId;
            playerColor.Value = Random.ColorHSV();
        }

        // Suscribirse a cambios
        playerColor.OnValueChanged += OnColorChanged;
        playerName.OnValueChanged += OnNameChanged;

        // Aplicar valores actuales (importante para clientes nuevos)
        ApplyColor(playerColor.Value);
        UpdateName(playerName.Value.ToString());
    }

    public override void OnDestroy()
{
    playerColor.OnValueChanged -= OnColorChanged;
    playerName.OnValueChanged -= OnNameChanged;

    base.OnDestroy();
}

    private void Update()
    {
        if (!IsOwner) return;

        // Presiona C para cambiar color
        if (Input.GetKeyDown(KeyCode.C))
        {
            ChangeColorServerRpc();
        }
    }

    // --- SERVER RPC PARA CAMBIAR COLOR ---
    [ServerRpc]
    private void ChangeColorServerRpc()
    {
        playerColor.Value = Random.ColorHSV();
    }

    // --- CALLBACKS DE NETWORKVARIABLE ---

    private void OnColorChanged(Color oldColor, Color newColor)
    {
        ApplyColor(newColor);
    }

    private void OnNameChanged(FixedString32Bytes oldName, FixedString32Bytes newName)
    {
        UpdateName(newName.ToString());
    }

    private void ApplyColor(Color color)
    {
        if (playerRenderer != null)
        {
            playerRenderer.material.color = color;
        }
    }

    private void UpdateName(string newName)
    {
        if (nameText != null)
        {
            nameText.text = newName;
        }
    }
    private void OnChangeColor(InputAction.CallbackContext context)
{
    if (!IsOwner) return;

    ChangeColorServerRpc();
}
    private void OnEnable()
{
    playerinput.Enable();
    playerinput.Player.cambiocolor.performed += OnChangeColor;
}

private void OnDisable()
{
    playerinput.Player.cambiocolor.performed -= OnChangeColor;
    playerinput.Disable();
}
}