using GameNetworkingShared.Objects;
using MultiplayerGameServer.Generic;
using MultiplayerGameServer.Server;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const float MoveSpeed = 20f / ServerConsts.TicksPerSecond;

    private float Angle { get; set; }

    private int Id { get; set; }

    private PlayerMovement Movement { get; set; } = new PlayerMovement();

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        UpdateMovement();
    }

    public void UpdateMovement()
    {
        float xDirection = (Movement.Right ? 1 : 0) + (Movement.Left ? -1 : 0);
        float yDirection = (Movement.Up ? 1 : 0) + (Movement.Down ? -1 : 0);

        if (xDirection != 0 || yDirection != 0)
        {
            Vector3 up = new Vector3(0, 1, 0);
            Vector3 right = new Vector3(1, 0, 0);
            Vector3 movement = Vector3.Normalize(up * yDirection + right * xDirection) * MoveSpeed;
            transform.position += movement;
        }

        transform.rotation = Quaternion.Euler(new Vector3(Angle, 90f, -90f));
        ServerSend.UpdatePlayerPositionRotation(new PlayerPosition(Id, transform.position, Angle));
    }

    public void SetMovement(PlayerMovement movement)
    {
        Movement = movement;
    }
}
