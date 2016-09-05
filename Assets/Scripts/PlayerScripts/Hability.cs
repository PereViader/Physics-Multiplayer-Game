using UnityEngine;
using System.Collections;

public abstract class Hability : Photon.MonoBehaviour {

    [SerializeField]
    protected float cooldown;

    protected float currentCooldown;
    protected bool onCooldown;
    protected string virtualKey;

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
        }
    }

    public void SetVirtualKey(string virtualKey)
    {
        this.virtualKey = virtualKey;
    }

    public abstract string GetHabilityName();
}
