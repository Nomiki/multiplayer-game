using MultiplayerGameServer.Generic;
using MultiplayerGameServer.Server;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float minTumble;
    public float maxTumble;
    public float minDistanceFromShips;
    public float minVelocity;
    public float maxVelocity;

    public GameObject[] possiblePrefabs;
    public CharacterController controller;
    private GameObject spawnedAsteroid;

    
    public float tumble;
    public float velocity;
    // Start is called before the first frame update
    void Start()
    {
        controller.detectCollisions = false;
        int selectedPrefabIndex = Random.Range(0, possiblePrefabs.Length);
        Vector3 position = CalculateAsteroidPosition();
        Quaternion rotation = CalculateAsteroidRotation();
        transform.position = position;
        transform.rotation = rotation;
        tumble = Random.Range(minTumble, maxTumble);
        velocity = Random.Range(minVelocity, maxVelocity);
        spawnedAsteroid = Instantiate(possiblePrefabs[selectedPrefabIndex], transform);
        spawnedAsteroid.GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;
    }
     
    private Quaternion CalculateVelocityAngle()
    {
        Player[] players = Server.Instance?.Clients?.Values?.Select(x => x.Player).ToArray();
        if (players == null)
        {
            return Quaternion.Euler(Random.Range(0f, 360f), 0f, 0f);
        }

        int selectedPlayer = Random.Range(0, players.Length);
        Player player = players[selectedPlayer];
        Vector3 dir = player.transform.position - transform.position;
        dir = player.transform.InverseTransformDirection(dir);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(angle, 0f, 0f);
    }

    private Quaternion CalculateAsteroidRotation()
    {
        float x = Random.Range(0, 360);
        float y = Random.Range(0, 360);
        float z = Random.Range(0, 360);

        return Quaternion.Euler(x, y, z);
    }

    private Vector3 CalculateAsteroidPosition()
    {
        bool positionCorrect = false;
        Vector3 positionCandidate = new Vector3();
        while (!positionCorrect)
        {
            positionCandidate = TryCalculateAsteroidPosition();
            positionCorrect = CheckPositionNotNearOtherObjects(positionCandidate);
        }

        return positionCandidate;
    }

    private bool CheckPositionNotNearOtherObjects(Vector3 positionCandidate)
    {
        Player[] players = Server.Instance?.Clients?.Values?.Select(x => x.Player).ToArray();
        if (players == null)
        {
            return true;
        }

        foreach(Player player in players)
        {
            if (Vector3.Distance(positionCandidate, player.transform.position) < minDistanceFromShips)
            {
                return false;
            }
        }

        return true;
    }

    private Vector3 TryCalculateAsteroidPosition()
    {
        float x = Random.Range(ServerConsts.NegativeBorder, ServerConsts.PositiveBorder);
        float y = Random.Range(ServerConsts.NegativeBorder, ServerConsts.PositiveBorder);

        return new Vector3(x, y, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDirection = new Vector3(transform.forward.x, transform.forward.y, 0f);
        controller.Move(velocity * moveDirection * Time.deltaTime);

        // todo: send data to users
    }
}
