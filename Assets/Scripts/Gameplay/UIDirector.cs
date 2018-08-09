using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDirector : MonoBehaviour
{
    public UIPlayer uiPlayer;
    public static UIDirector instance;
    // Use this for initialization
    void Start ()
    {
		if (instance != null)
        {

        }
        instance = this;
    }

    public void UIInit()
    {
        SetPlayerUI(GameDirector.instance.player.currentHealth, GameDirector.instance.player.currentStamina, GameDirector.instance.player.currentVitality);
    }

    public void SetPlayerUI(float health, float stamina, float vitality)
    {
        uiPlayer.Init(health, stamina, vitality);
    }
}
