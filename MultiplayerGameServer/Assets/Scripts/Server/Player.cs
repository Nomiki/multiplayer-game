using GameNetworkingShared.Logging;
using GameNetworkingShared.Objects;
using MultiplayerGameServer.Generic;
using MultiplayerGameServer.Server;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController controller;

    private float Angle { get; set; }

    public int Id { get; set; }

    private float acceleration = 100f;
    private float speedLimit = 100f;
    private float xVelocity = 0;
    private float yVelocity = 0;

    private PlayerMovement Movement { get; set; } = new PlayerMovement();

    void Start()
    {
        acceleration = acceleration * Time.fixedDeltaTime * Time.fixedDeltaTime;
    }

    void FixedUpdate()
    {
        UpdateMovement();
        Debug.DrawRay(transform.position, transform.forward * 90, Color.red);
    }

    public void UpdateMovement()
    {
        float xDirection = (Movement.Right ? 1f : 0f) + (Movement.Left ? -1f : 0f);
        float yDirection = (Movement.Up ? 1f : 0f) + (Movement.Down ? -1f : 0f);

        xVelocity = CalculateVelocity(xVelocity, xDirection);
        yVelocity = CalculateVelocity(yVelocity, yDirection);

        Vector3 moveDirection = new Vector3(xVelocity, yVelocity, 0f);
        controller.Move(moveDirection);

        transform.rotation = Quaternion.Euler(new Vector3(Angle, 90f, -90f));
        ServerSend.UpdatePlayerPositionRotation(new PlayerPosition(Id, transform.position, Angle));
    }

    private float CalculateVelocity(float currentVelocity, float direction)
    {
        if (direction == 0f)
        {
            float currentSign = Mathf.Sign(currentVelocity);
            float stopVelocity = currentVelocity - acceleration * currentSign;
            float afterVelocityChangeSign = Mathf.Sign(stopVelocity);
            return currentSign != afterVelocityChangeSign ? 0f : stopVelocity;
        }

        float newVelocity = currentVelocity + direction * (acceleration * 2f);
        if (Mathf.Abs(newVelocity) > speedLimit)
        {
            newVelocity = speedLimit;
        }

        return newVelocity;
    }

    public void SetMovement(PlayerMovement movement)
    {
        Movement = movement;
        Angle = Movement.Angle;
    }
}
