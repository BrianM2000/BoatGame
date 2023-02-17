using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UpgradeBaseSO : ScriptableObject
{
    public Image picture;
    public int tier;
    //bonus is flat boost i.e. healthBonus = 10 -> health + 10
    //Multipler is multipler i.e. healthMultiper = 1.1 -> health * 1.1
    public float cannonLoadSpeedBonus = 0;
    public float cannonLoadSpeedMultipler = 1;
    public float cannonAccuracyBonus = 0;
    public float cannonAccuracyMultipler = 1;
    public float shipHealthBonus = 0;
    public float shipHealthMultipler = 1;
    public float shipSpeedBonus = 0;
    public float shipSpeedMultipler = 1;
    public float shipSailsBonus = 0;
    public float shipSailsMultipler = 1;
    public float shipBailSpeedBonus = 0;
    public float shipBailSpeedMultipler = 1;
    public float shipRepairSpeedBonus = 0;
    public float shipRepairSpeedMultipler = 1;
    public float sailorHealthBonus = 0;
    public float sailorHealthMultipler = 1;
    public float sailorWorkSpeedBonus = 0;
    public float sailorWorkSpeedMultipler = 1;
}
