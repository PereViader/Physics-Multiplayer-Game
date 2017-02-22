using UnityEngine;
using UnityEngine.UI;

using System.Collections;

[System.Serializable]
public class UI_Hability {

    [SerializeField]
    private Text nameText;

    [SerializeField]
    private Text cooldownText;

    public void setName(string name)
    {
        nameText.text = name;
    }

    public void setCooldown(float cooldown)
    {
        cooldownText.text = cooldown.ToString("0.0");
    }
}
