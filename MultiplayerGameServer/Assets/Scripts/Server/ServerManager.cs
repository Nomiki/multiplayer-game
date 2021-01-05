using GameNetworkingShared.Logging;
using GameNetworkingShared.Objects;
using GameNetworkingShared.Protocols;
using GameNetworkingShared.Threading;
using MultiplayerGameServer.Logging;
using MultiplayerGameServer.Server;
using System;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    public static ServerManager Instance { get; set; }

    public GameObject PlayerPrefab;

    private void Awake()
    {

#if UNITY_EDITOR
        UnityLogger.Initiate();
#else
        ConsoleLogger.Initiate();
#endif

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

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        Server.Instance.Start(5, Constants.ServerPort);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        TaskManager.Instance.RunQueuedTasks();
    }

    internal Player InstatiatePlayer(PlayerPacket playerPacket)
    {
        Vector3 position = playerPacket.Position.GetPosition();
        Quaternion rotation = playerPacket.Position.GetRotation();

        GameObject playerObj = Instantiate(PlayerPrefab, position, rotation, transform);
        Player player = playerObj.GetComponent<Player>();
        player.Id = playerPacket.Id;
        return player;
    }

    private void OnApplicationQuit()
    {
        Server.Instance.Dispose();
    }
}
