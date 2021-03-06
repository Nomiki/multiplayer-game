﻿using Assets.Scripts.Client;
using GameNetworkingShared.Logging;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        public GameObject StartMenu;
        public InputField UsernameField;
        public GameObject ShipSelectionGameObject;
        public GameObject[] ShipPrefabs;
        public int Selection = 0;

        private void Awake()
        {
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

        public void ConnectToServer()
        {
            StartMenu.SetActive(false);
            UsernameField.interactable = false;
            ShipSelectionGameObject.SetActive(false);

            ClientManager.Instance.ConnectToServer();
        }
    }
}
