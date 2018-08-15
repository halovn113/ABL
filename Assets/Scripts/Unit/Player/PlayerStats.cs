using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float staminaRecoverPerSec; // số stamina hồi phục trên 1/60 giây
    public float vitalityLosePerSec; // số vitality bị mất trên 1/60 giây

    public float healthPer; // chỉ số sức khỏe đẻ nhân với các chỉ số physical, strength và endurance

    public float moveSpeed;
    public float maxSpeed;

    public int physical;
    public int strength;
    public int agility;
    public int knowledge;
    public int morale;
    public int endurable;

}
