using Assets.Scripts.Logging;
using GameNetworkingShared.Logging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSelectionController : MonoBehaviour
{
    public GameObject[] ShipPrefabs;

    public int Selection = 0;

    private const float Speed = 100f;
    private GameObject rotatingShip;

    private void Awake()
    {
        UnityLogger.Initiate();
        LogFactory.Instance.Debug($"{ShipPrefabs.Length} ships loaded");

        SpawnShip();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.down * Speed * Time.deltaTime);
    }

    void SpawnShip()
    {
        if (rotatingShip != null)
        {
            Destroy(rotatingShip);
        }

        rotatingShip = Instantiate(ShipPrefabs[Selection], this.transform);
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
