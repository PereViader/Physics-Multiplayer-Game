using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementController : Photon.MonoBehaviour {

    [SerializeField]
    private float startingForce;

    [SerializeField]
    private float maxForce;

    [SerializeField]
    private float timeToGetFullForce;

    [SerializeField]
    private float chargeCooldown;

    [HideInInspector]
    public float currentPercentage;
    [HideInInspector]
    public bool isChargingMove;


    private Rigidbody rb;
    

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (InputState.isGameInput && Input.GetAxis("Fire1") > 0)
        {
            StartCoroutine(ExecutePlayerMovement());
        }
    }

    IEnumerator ExecutePlayerMovement()
    {
        enabled = false;
        isChargingMove = true;
        currentPercentage = 0;
        float percentageIncrementPerSecond = 1/timeToGetFullForce;

        // mentre es carrega i no es cancela
        while(Input.GetAxis("Fire1") > 0 && Input.GetAxis("Fire2") == 0)
        {
            currentPercentage = Mathf.Clamp01(currentPercentage + percentageIncrementPerSecond * Time.deltaTime);
            yield return null;
        }
        
        // si no s'ha cancelat vol dir que s'ha disparat
        if (Input.GetAxis("Fire2") == 0)
        {
            // move has been charged and has to execute
            //Vector3 force = InputGetter.GetDirection() * currentForce;
            Vector3 force = Camera.main.transform.forward * maxForce*currentPercentage;
            photonView.RPC("ShootPlayer", PhotonTargets.MasterClient, force);
        }
        yield return new WaitForSeconds(chargeCooldown);
        isChargingMove = false;
        enabled = true;
    }

    [PunRPC]
    public void ShootPlayer(Vector3 force)
    {
        GetComponent<PhotonTransformView>().SetSynchronizedValues(transform.position,0);
        rb.AddForce(force, ForceMode.Impulse);
    }

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        enabled = PhotonNetwork.player.ID == (int)photonView.instantiationData[0];
    }
}
