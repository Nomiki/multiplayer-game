using Assets.Scripts.Client;
using GameNetworkingShared.Logging;
using GameNetworkingShared.Objects;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool IsSelf = false;

    private MouseController MouseController { get; set; }

    private void Start()
    {
        MouseController = GetComponent<MouseController>();
    }

    private void Update()
    {
        if (IsSelf)
        {
            SendInputToServer();
        }
    }

    private void SendInputToServer()
    {
        PlayerMovement movement = new PlayerMovement()
        {
            Up = Input.GetKey(KeyCode.W),
            Down = Input.GetKey(KeyCode.S),
            Left = Input.GetKey(KeyCode.A),
            Right = Input.GetKey(KeyCode.D),
            Angle = MouseController?.Angle ?? 0f
        };

        ClientSend.SendPlayerMovement(movement);
    }


}
