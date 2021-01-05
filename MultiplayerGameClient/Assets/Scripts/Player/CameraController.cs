using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Player;

    private void Start()
    {
        transform.position = new Vector3(Player.position.x, Player.position.y, ClientConstants.CameraOffset);
    }

    private void FixedUpdate()
    {
    }
}
