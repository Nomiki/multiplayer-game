using GameNetworkingShared.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int Id;
    public string Username;

    public void SetPlayerData(Player playerData)
    {
        Id = playerData.Id;
        Username = playerData.Username;
    }
}
