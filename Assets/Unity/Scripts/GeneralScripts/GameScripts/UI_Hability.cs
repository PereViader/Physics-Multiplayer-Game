using UnityEngine;
using UnityEngine.UI;

using System.Collections;

[System.Serializable]
public class UI_Hability {

    [SerializeField]
    private Text nameText;

    [SerializeField]
    private Text cooldownText;

    private Hability hability;

    public void SetHability(Hability hability)
    {
        this.hability = hability;
        nameText.text = hability.habilityName;
    }

    public void Update()
    {
        if ( hability != null)
        {
            cooldownText.text = hability.currentCooldown.ToString("0.0");
        }
    }
}
