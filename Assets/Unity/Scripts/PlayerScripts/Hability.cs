using UnityEngine;
using System.Collections;

public abstract class Hability : Photon.MonoBehaviour {

    [SerializeField]
    protected float cooldown;

    protected float currentCooldown;
    protected bool onCooldown;
    protected string virtualKey;
    protected int habilityNumber;

    protected UI_Hability habilityUi;

    public void Initialize(UI_Hability habilityUi, int habilityNumber)
    {
        //link hability to it's UI representation
        this.habilityUi = habilityUi;
        habilityUi.setName(GetHabilityName());

        // link Hability to it's Input
        // virtual key starts at one, arrays start at 0
        this.virtualKey = "Hability" + (habilityNumber + 1);
    }

    public float GetCurrentCooldown()
    {
        return currentCooldown;
    }

    public void SetCooldown(float cooldown)
    {
        this.cooldown = cooldown;
    }

    protected void SetOnCooldown()
    {
        habilityUi.setCooldown(cooldown);
        currentCooldown = cooldown;
        onCooldown = true;
    }

    protected virtual void Update()
    {
        if ( onCooldown )
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0f)
            {
                onCooldown = false;
                currentCooldown = 0f;
            }
            habilityUi.setCooldown(currentCooldown);
        }
    }

    public void SetHabilityNumber()
    {
        
    }

    public abstract string GetHabilityName();
}
