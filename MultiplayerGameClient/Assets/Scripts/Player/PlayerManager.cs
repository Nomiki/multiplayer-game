using Assets.Scripts.UI;
using GameNetworkingShared.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int Id;
    public string Username;
    public int ShipModelId;

    public void SetPlayerDataAndSpawnShipModel(Player playerData)
    {
        Id = playerData.Id;
        Username = playerData.Username;
        ShipModelId = playerData.ShipModelId;

        GameObject shipModel = UIManager.Instance.ShipPrefabs[ShipModelId];
        Instantiate(shipModel, this.transform);
    }
}
