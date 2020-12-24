using Assets.Scripts.Client;
using Assets.Scripts.Logging;
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

    public GameObject localPlayerPrefab;
    public GameObject remotePlayerPrefab;

    private void Awake()
    {
        UnityLogger.Initiate();
        LogFactory.Instance.Debug("Greetings!");
        if (Instance == null)
        {
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
        GameObject chosenPrefab = playerData.Id == ClientManager.Instance.Client.Id
            ? localPlayerPrefab
            : remotePlayerPrefab;

        Vector3 position = new Vector3(playerData.Position.X, playerData.Position.Y, playerData.Position.Z);
        Quaternion rotation = Quaternion.AngleAxis(playerData.Position.Angle, Vector3.forward);
        GameObject player = Instantiate(chosenPrefab, position, rotation);
    }
}
