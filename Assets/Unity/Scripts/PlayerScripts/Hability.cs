using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class Hability : Photon.MonoBehaviour {

    protected float cooldown;

    public float currentCooldown
    {
        get;
        private set;
    }

    public bool onCooldown
    {
        get;
        private set;
    }

    public string habilityName
    {
        get;
        protected set;
    }

    private string habilityButton;
    private int habilityNumber;

    protected PhotonTargets habilityTarget = PhotonTargets.MasterClient;

    public void Initialize(int habilityNumber)
    {
        this.habilityNumber = habilityNumber;
        this.habilityButton = "Hability"+habilityNumber;
    }

    protected void SetOnCooldown()
    {
        currentCooldown = cooldown;
        onCooldown = true;
    }

    protected void UpdateCooldown()
    {
        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0f)
        {
            onCooldown = false;
            currentCooldown = 0f;
        }
    }

    public virtual void Update()
    {
        if ( onCooldown )
        {
            UpdateCooldown();
        } else if (Input.GetButtonDown(habilityButton))
        {
            SetOnCooldown();
            ExecuteHabilityRPC();
        }
    }

    public virtual void ExecuteHabilityRPC()
    {
        //executat a habilityManager que fa de hub per unificar les crides rpc de totes les habilitats
        //s'ha de fer aixi per evitar problemes
        photonView.RPC("ExecuteHability", habilityTarget, habilityNumber);
    }

    public abstract void ExecuteHability();
}
