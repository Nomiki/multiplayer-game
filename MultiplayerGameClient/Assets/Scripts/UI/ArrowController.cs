using GameNetworkingShared.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class ArrowController : MonoBehaviour
    {
        public GameObject ArrowImagePrefab;

        private Dictionary<int, GameObject> Arrows = new Dictionary<int, GameObject>();

        public static ArrowController Instance { get; set; }


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

        private void FixedUpdate()
        {
            PlayerManager currentPlayer = GameManager.Instance.Players.Values.FirstOrDefault(x => x.Id == Client.ClientManager.Instance.Client.Id);

            if (currentPlayer == null)
                return;

            Vector2 currentPlayerPositionOnScreen = Camera.main.WorldToViewportPoint(currentPlayer.transform.position);

            foreach (PlayerManager player in GameManager.Instance.Players.Values.Where(x => x.Id != Client.ClientManager.Instance.Client.Id))
            {
                Vector2 otherPlayerPositionOnScreen = Camera.main.WorldToViewportPoint(player.transform.position);

                if (otherPlayerPositionOnScreen.x >= 0 && otherPlayerPositionOnScreen.x <= 1 && otherPlayerPositionOnScreen.y >= 0 && otherPlayerPositionOnScreen.y <= 1)
                {
                    // ship is on screen bounds...
                    if (Arrows.ContainsKey(player.Id))
                    {
                        Arrows[player.Id].SetActive(false);
                    }

                    return;
                }

                if (!Arrows.ContainsKey(player.Id))
                {
                    var image = Instantiate(ArrowImagePrefab, transform);
                    Arrows[player.Id] = image;
                }

                Arrows[player.Id].SetActive(true);
                var angle = AngleBetweenTwoPoints(currentPlayerPositionOnScreen, otherPlayerPositionOnScreen);
                Arrows[player.Id].transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));

                float arrowPosX = Mathf.Clamp(otherPlayerPositionOnScreen.x - 0.5f, -0.5f, 0.5f) * (Screen.width - 100);
                float arrowPosY = Mathf.Clamp(otherPlayerPositionOnScreen.y - 0.5f, -0.5f, 0.5f) * (Screen.height - 100);
                Arrows[player.Id].transform.position = new Vector3((Screen.width * 0.5f) + arrowPosX, (Screen.height * 0.5f) + arrowPosY, 0);
            }
        }

        internal void RemoveArrow(int id)
        {
            if (Arrows.ContainsKey(id))
            {
                Arrows[id].SetActive(false);
            }
        }

        private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
        {
            return (Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg);
        }
    }
}
