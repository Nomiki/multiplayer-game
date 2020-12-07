using UnityEngine;

public class ThreadManager : MonoBehaviour
{
    private void Update()
    {
        GameNetworkingShared.Threading.ThreadManager.UpdateMain();
    }
}