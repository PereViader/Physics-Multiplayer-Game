using UnityEngine;
using System.Collections;

public class HabilityShrink : Hability {

    [SerializeField]
    float shrinkedDivider;

    [SerializeField]
    float shrinkDuration;

    Vector3 previousScale;

    void Awake()
    {
        cooldown = 5f;
        shrinkDuration = 2f;
        shrinkedDivider = 2f;
        habilityName = "Shrink";
        habilityTarget = PhotonTargets.All;
    }

    public override void ExecuteHability()
    {
        StartCoroutine(HabilityCoroutine());
    }

    IEnumerator HabilityCoroutine()
    {
        previousScale = transform.localScale;
        transform.localScale = transform.localScale / shrinkedDivider;
        yield return new WaitForSeconds(shrinkDuration);
        transform.localScale = previousScale;
    }
}
