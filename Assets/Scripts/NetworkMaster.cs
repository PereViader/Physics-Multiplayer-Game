using UnityEngine;
using UnityEngine.Networking;


public class NetworkMaster : NetworkBehaviour {
    /**
    [SerializeField]
    private PlayerController playerController;

    [Command]
    public void CmdMovePlayer(NetworkPlayer networkPlayer, Vector3 force)
    {
        float ping = Network.GetLastPing(networkPlayer);
        float delay = ping * 2 / 3;
        double currentTime = Network.time;

        RpcMovePlayer(currentTime + delay, force);
    }

    [ClientRpc]
    public void RpcMovePlayer(double movementTime, Vector3 force)
    {
        double currentTime = Network.time;
        if (currentTime > movementTime)
        {
            // calcular la posicio correcte de la fitxa, teletransportar i donar velocitat
        }
        else {
            playerController.ShootPlayer(force);
        }
    }**/
}
