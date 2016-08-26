using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class PlayerControllerPast : Photon.MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float shootCooldown;

    private ShootingController shootingController;
    private Rigidbody rb;

    bool isShooting = false;
    bool isInputEnabled = true;


    Vector3 surfaceVector;
    void Awake()
    {
        CaptureEvents.OnLocalPlayerSpawned += SetOwnPlayer;
        shootingController = GetComponent<ShootingController>();
        rb = GetComponent<Rigidbody>();
        surfaceVector = Quaternion.Euler(new Vector3(0, 0, 45)) * Vector3.right * transform.localScale.x;
    }

    void OnDestroy()
    {
        CaptureEvents.OnLocalPlayerSpawned -= SetOwnPlayer;
    }

    void Update()
    {
        if (!PauseMenuManager.isPausePanelActive)
        {
            if (!isShooting && Input.GetAxis("Fire1") > 0 && isInputEnabled)
            {
                StartCoroutine(CaptureInputAndShoodPlayer());
            }
        }
    }

    IEnumerator CaptureInputAndShoodPlayer()
    {
        isShooting = true;
        shootingController.SetActive(true);

        while (Input.GetAxis("Fire1") > 0 && !(Input.GetAxis("Fire2")>0))
        {
            yield return null;
        }

        if (Input.GetAxis("Fire2") > 0) // canceled move. whait until fire1 has been released
        {
            shootingController.SetActive(false);
            while (Input.GetAxis("Fire1") > 0)
            {
                yield return null;
            }
        } else  // input capture done execute movement
        {
            float power = shootingController.getPower();
            shootingController.SetActive(false);

            Vector3 force = InputGetter.GetDirection() * power * speed;

            GetComponent<PhotonView>().RPC("ShootPlayer", PhotonTargets.MasterClient, force);
            yield return new WaitForSeconds(shootCooldown);
        }
        isShooting = false;
    }


    [PunRPC]
    public void ShootPlayer(Vector3 force)
    {
        Vector2 forceDirection = new Vector2(force.normalized.x, force.normalized.z).normalized;
        float angleBetween = Vector2.Angle(Vector2.right, forceDirection);
        Quaternion rotationToSurface = Quaternion.Euler(0f, angleBetween, 0f);
        Vector3 surfacePosition = rotationToSurface * surfaceVector;
        rb.angularVelocity = Vector3.zero;
        rb.AddForceAtPosition(force, transform.position + surfacePosition, ForceMode.Impulse);
    }

    public void SetInput(bool isActive)
    {
        isInputEnabled = isActive;
    }

    public void SetOwnPlayer(GameObject other)
    {
        enabled = other == gameObject;
    }
}
