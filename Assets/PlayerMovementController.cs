using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementController : MonoBehaviour {

    [SerializeField]
    private float startingForce;

    [SerializeField]
    private float forceIncrementPerSecond;

    [SerializeField]
    private float maxForce;

    [SerializeField]
    private float chargeCooldown;

    private float currentForce;
    private bool isChargingMove;

    private Vector3 surfaceVector;

    private Rigidbody rb;
    private PhotonView photonView;
    private UI_PowerBarManager powerBar;
    

    void Awake()
    {
        isChargingMove = false;
        rb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
        surfaceVector = Quaternion.Euler(new Vector3(0, 0, 45)) * Vector3.right * transform.localScale.x;
        powerBar = GameObject.FindObjectOfType<UI_PowerBarManager>();
        Debug.Log(powerBar);
    }

    void Update()
    {
        if (!isChargingMove && Input.GetAxis("Fire1") > 0)
        {
            StartCoroutine(ExecutePlayerMovement());
        }
    }

    IEnumerator ExecutePlayerMovement()
    {
        isChargingMove = true;
        currentForce = startingForce-forceIncrementPerSecond;
        Debug.Log("Start SHoot");
        // send event to notify UI that it has to start to display
        // set its value
        powerBar.setDisplay(true);

        while(Input.GetAxis("Fire1") > 0 && Input.GetAxis("Fire2") == 0)
        {
            currentForce = Mathf.Clamp(currentForce + forceIncrementPerSecond * Time.deltaTime, startingForce, maxForce);
            powerBar.setFill(currentChargePercentage());
            yield return null;
        }
        powerBar.setDisplay(false);

        if (Input.GetAxis("Fire2") > 0)
        {
            // move has been canceled
            // wait until the button has been released to not start charging at the start of the next frame
            while (Input.GetAxis("Fire1") > 0)
            {
                yield return null;
            }
        } else
        {
            // move has been charged and has to execute
            Vector3 force = InputGetter.GetDirection() * currentForce;
            photonView.RPC("ShootPlayer", PhotonTargets.MasterClient, force);
            yield return new WaitForSeconds(chargeCooldown);
        }
        isChargingMove = false;
    }

    float currentChargePercentage()
    {
        return (currentForce - startingForce) / (maxForce - startingForce);
    }

    [PunRPC]
    public void ShootPlayer(Vector3 force)
    {
        Debug.Log("SHoot execute "+force);
        Vector2 forceDirection = new Vector2(force.normalized.x, force.normalized.z).normalized;
        float angleBetween = Vector2.Angle(Vector2.right, forceDirection);
        Quaternion rotationToSurface = Quaternion.Euler(0f, angleBetween, 0f);
        Vector3 surfacePosition = rotationToSurface * surfaceVector;
        rb.angularVelocity = Vector3.zero;
        rb.AddForceAtPosition(force, transform.position + surfacePosition, ForceMode.Impulse);
    }
}
