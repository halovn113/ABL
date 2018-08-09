using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayer : MonoBehaviour
{
    public UIBar Health;
    public UIBar Vitality;
    public UIBar Stamina;

    public void Init(float health, float stamina, float vitality)
    {
        Health.UIBarInit();
        Vitality.UIBarInit();
        Stamina.UIBarInit();

        Health.UpdateBar(health);
        Stamina.UpdateBar(stamina);
        Vitality.UpdateBar(vitality);
    }
}
