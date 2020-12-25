using Assets.Scripts.Client;
using Assets.Scripts.Logging;
using Assets.Scripts.UI;
using GameNetworkingShared.Logging;
using GameNetworkingShared.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{   
    public static GameManager Instance { get; set; }

    public static Dictionary<int, PlayerManager> Players = 
        new Dictionary<int, PlayerManager>();

    public GameObject playerPrefab;
    public GameObject gameCamera;

    private void Awake()
    {
        UnityLogger.Initiate();
        if (Instance == null)
        {
            LogFactory.Instance.Debug($"loaded {this.GetType()}: {this}");
            Instance = this;
        }
        else if (Instance != this)
        {
            LogFactory.Instance.Debug("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void SpawnPlayer(Player playerData)
    {
        Vector3 position = new Vector3(playerData.Position.X, playerData.Position.Y, playerData.Position.Z);
        Quaternion rotation = Quaternion.AngleAxis(playerData.Position.Angle, Vector3.forward);
        GameObject player = Instantiate(playerPrefab, position, rotation, this.transform);

        if (playerData.Id == ClientManager.Instance.Client.Id)
        {
            gameCamera.GetComponent<CameraController>().Player = player.transform;
        }

        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        if (playerManager != null)
        {
            playerManager.SetPlayerDataAndSpawnShipModel(playerData);
            Players.Add(playerData.Id, playerManager);
        }
    }
}
