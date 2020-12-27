﻿using Assets.Scripts.UI;
using GameNetworkingShared.Objects;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int Id;
    public string Username;
    public int ShipModelId;
    private MouseController MouseController;

    private void Start()
    {
        MouseController = GetComponent<MouseController>();
    }

    public void SetPlayerPosition(PlayerPosition position)
    {
        transform.position = new Vector3(position.X, position.Y, position.Z);
        MouseController.SetRemoteAngle(position.Angle);
    }

    public void SetPlayerDataAndSpawnShipModel(Player playerData)
    {
        Id = playerData.Id;
        Username = playerData.Username;
        ShipModelId = playerData.ShipModelId;

        GameObject shipModel = UIManager.Instance.ShipPrefabs[ShipModelId];
        Instantiate(shipModel, this.transform);
    }
}
