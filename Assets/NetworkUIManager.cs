using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System.Net;
using System.Net.Sockets;

public class NetworkUIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject panelMenu;
    public GameObject panelHost;
    public GameObject panelCliente;

    [Header("UI")]
    public TMP_Text ipText;
    public TMP_InputField ipInput;

    private UnityTransport transport;

    void Start()
    {
        transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

        panelMenu.SetActive(true);
        panelHost.SetActive(false);
        panelCliente.SetActive(false);
    }

    // BOTÓN HOST
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();

        string localIP = GetLocalIPAddress();
        ipText.text = "IP: " + localIP;

        panelMenu.SetActive(false);
        panelHost.SetActive(true);
    }

    // BOTÓN CLIENTE (mostrar panel)
    public void ShowClientPanel()
    {
        panelMenu.SetActive(false);
        panelCliente.SetActive(true);
    }

    // BOTÓN CONECTAR
    public void StartClient()
    {
        string ip = ipInput.text;

        transport.ConnectionData.Address = ip;
        NetworkManager.Singleton.StartClient();
    }

    // Obtener IP local
    private string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        return "No IP Found";
    }
}
