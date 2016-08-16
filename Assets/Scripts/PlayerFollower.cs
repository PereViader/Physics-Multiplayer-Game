using UnityEngine;
using System.Collections;

public class PlayerFollower : MonoBehaviour {

    [SerializeField]
    private Transform player;

    [SerializeField]
    private float lerpFactor = 0.1f;

    [SerializeField]
    float teleportDistance;

    [SerializeField]
    bool canTeleport;

    void FixedUpdate()
    {
        if (player)
        {
            if (canTeleport && Vector3.Distance(transform.position, player.position) > teleportDistance)
                transform.position = player.position;
            else
                transform.position = Vector3.Lerp(transform.position, player.position, lerpFactor * Time.fixedDeltaTime);
        }
        else
            Destroy(gameObject);
    }

    public void setPlayer(GameObject toFollow)
    {
        player = toFollow.transform;
    }
}
