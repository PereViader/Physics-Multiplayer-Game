using UnityEngine;
using System.Collections;

public class HabilityShrink : Hability {

    [SerializeField]
    float shrinkedDivider;

    [SerializeField]
    float shrinkDuration;

    Vector3 previousScale;

    private bool isBlocked = false;

    void Awake()
    {
        cooldown = 5f;
        shrinkDuration = 2f;
        shrinkedDivider = 2f;
    }

    protected override void Update()
    {
        base.Update();
        if (!onCooldown && !isBlocked && Input.GetButtonDown(virtualKey))
        {
            SetOnCooldown();
            photonView.RPC("ExecuteShrink", PhotonTargets.AllViaServer);
        }
    }

    [PunRPC]
    void ExecuteShrink()
    {
        previousScale = transform.localScale;
        transform.localScale = transform.localScale/shrinkedDivider;
        Invoke("EndShrink", shrinkDuration);
    }

    void EndShrink()
    {
        transform.localScale = previousScale;
    }

    public override string GetHabilityName()
    {
        return "Shrink";
    }
}
