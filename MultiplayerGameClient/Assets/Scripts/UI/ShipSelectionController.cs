using Assets.Scripts.UI;
using GameNetworkingShared.Logging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSelectionController : MonoBehaviour
{
    private const float Speed = 100f;
    private bool initiated = false;
    private GameObject rotatingShip;

    private GameObject[] ShipPrefabs => UIManager.Instance.ShipPrefabs;
    private int Selection { get => UIManager.Instance.Selection; set => UIManager.Instance.Selection = value; }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.down * Speed * Time.deltaTime);
        if (!initiated)
        {
            SpawnShip();
            initiated = true;
        }
    }

    void SpawnShip()
    {
        if (rotatingShip != null)
        {
            Destroy(rotatingShip);
        }

        GameObject shipPrefab = ShipPrefabs[Selection];
        rotatingShip = Instantiate(shipPrefab, this.transform);
        rotatingShip.layer = 5;
    }

    public void SetNextShip()
    {
        Selection = (Selection + 1) % ShipPrefabs.Length;
        SpawnShip();
    }

    public void SetPrevShip()
    {
        Selection = (Selection - 1 + ShipPrefabs.Length) % ShipPrefabs.Length;
        SpawnShip();
    }
}
