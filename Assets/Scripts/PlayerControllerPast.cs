using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class PlayerControllerPast : Photon.MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float _timeBetweenShoots;
    [SerializeField]
    private double timeBeforeShoots;

    private ShootingController shootingController;
    private Rigidbody rb;


    private Vector3 currentMovementForce;

    bool isOwnPlayer = false;
    bool isShooting = false;
    bool isInputEnabled = true;

    void Awake()
    {
        shootingController = GameObject.Find("Canvas/ShootingController").GetComponent<ShootingController>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isOwnPlayer)
        {
            if (!isShooting && Input.GetAxis("Fire1") > 0 && isInputEnabled)
            {
                StartCoroutine(CaptureInputAndShoodPlayer());
            }
        }
    }

    IEnumerator CaptureInputAndShoodPlayer()
    {
        shootingController.SetActive(true);
        isShooting = true;

        while (Input.GetAxis("Fire1") > 0)
            yield return null;

        float power = shootingController.getPower();
        shootingController.SetActive(false);

        Vector3 force = InputGetter.GetDirection() * power * speed;

        GetComponent<PhotonView>().RPC("ShootPlayer", PhotonTargets.MasterClient, force);
        yield return new WaitForSeconds(_timeBetweenShoots);
        isShooting = false;
    }

    [PunRPC]
    public void ShootPlayer(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
    }

    public void SetInput(bool isActive)
    {
        isInputEnabled = isActive;
    }

    public void SetOwnPlayer(bool isOwn)
    {
        isOwnPlayer = isOwn;
    }

    void OnDestroy()
    {

        shootingController.SetActive(false);
    }
}
