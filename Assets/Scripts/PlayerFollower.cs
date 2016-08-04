using UnityEngine;
using System.Collections;

public class PlayerFollower : MonoBehaviour {

    [SerializeField]
    private Transform player;

    [SerializeField]
    private float lerpFactor = 0.1f;

    void FixedUpdate()
    {
        if (player)
            transform.position = Vector3.Lerp(transform.position, player.position, lerpFactor*Time.fixedDeltaTime);
        else
            Debug.Log("Player is null");
    }

    public void setPlayer(GameObject toFollow)
    {
        player = toFollow.transform;
    }
}
