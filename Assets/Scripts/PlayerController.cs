using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class PlayerController : Photon.MonoBehaviour {



    [SerializeField]
    private float speed;
    [SerializeField]
    private float _timeBetweenShoots;
    [SerializeField]
    private double timeBeforeShoots;

    private ShootingController shootingController;
    private Rigidbody rb;

    private int currentMovementId;
    private double currentMovementTimeToShoot = -1;
    private Vector3 currentMovementForce;
    public bool isOwnPlayer = false;
    private bool isShooting = false;

    void Awake()
    {
        currentMovementId = 0;
        shootingController = GameObject.Find("Canvas/ShootingController").GetComponent<ShootingController>();
        rb = GetComponent<Rigidbody>();
    }

	void Update ()
    {
        if (currentMovementTimeToShoot > 0)
        {
            if (currentMovementTimeToShoot <= PhotonNetwork.time)
            {
                currentMovementTimeToShoot = -1;
                ShootPlayer(currentMovementForce,currentMovementId);
            }
        }
        else if (isOwnPlayer)
        {
            if (!isShooting && Input.GetAxis("Fire1") > 0)
            {
                StartCoroutine(CaptureInputAndShoodPlayer());
            }
        }
    }

    IEnumerator CaptureInputAndShoodPlayer()
    {
        if (!shootingController.activate())
            yield break;

        isShooting = true;

        while (Input.GetAxis("Fire1") > 0)
            yield return null;

        float power = shootingController.getPower();

        Vector3 force = InputGetter.GetDirection() * power * speed;

        GetComponent<PhotonView>().RPC("ShootPlayerAtTime", PhotonTargets.All, force, PhotonNetwork.time+ timeBeforeShoots, currentMovementId+1);
        yield return new WaitForSeconds(_timeBetweenShoots);
        isShooting = false;
    }

    [PunRPC]
    void ShootPlayerAtTime(Vector3 force, double timeToShoot, int moveId)
    {
        currentMovementTimeToShoot = timeToShoot;
        currentMovementForce = force;
        currentMovementId = moveId;
    }

    [PunRPC]
    public void ShootPlayer(Vector3 force, int moveId )
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(force, ForceMode.Impulse);
    }

    public int getCurrentMovementId()
    {
        return currentMovementId;
    }
}
