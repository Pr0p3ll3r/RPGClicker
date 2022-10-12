using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "RPG/Item/Equipment")]
public class EquipmentBase : Equipment
{
    [Header("Damage")]
    public int damageMinCommon;
    public int damageMaxCommon;
    public int damageMinRare;
    public int damageMaxRare;
    public int damageMinEpic;
    public int damageMaxEpic;
    public int damageMinLegendary;
    public int damageMaxLegendary;

    [Header("Defense")]
    public int defenseMinCommon;
    public int defenseMaxCommon;
    public int defenseMinRare;
    public int defenseMaxRare;
    public int defenseMinEpic;
    public int defenseMaxEpic;
    public int defenseMinLegendary;
    public int defenseMaxLegendary;

    [Header("Strength")]
    public int strengthMinCommon;
    public int strengthMaxCommon;
    public int strengthMinRare;
    public int strengthMaxRare;
    public int strengthMinEpic;
    public int strengthMaxEpic;
    public int strengthMinLegendary;
    public int strengthMaxLegendary;

    [Header("Vitality")]
    public int vitalityMinCommon;
    public int vitalityMaxCommon;
    public int vitalityMinRare;
    public int vitalityMaxRare;
    public int vitalityMinEpic;
    public int vitalityMaxEpic;
    public int vitalityMinLegendary;
    public int vitalityMaxLegendary;

    [Header("Critical Damage")]
    public int criticalDamageMinCommon;
    public int criticalDamageMaxCommon;
    public int criticalDamageMinRare;
    public int criticalDamageMaxRare;
    public int criticalDamageMinEpic;
    public int criticalDamageMaxEpic;
    public int criticalDamageMinLegendary;
    public int criticalDamageMaxLegendary;

    [Header("Critical Chance")]
    public int criticalChanceMinCommon;
    public int criticalChanceMaxCommon;
    public int criticalChanceMinRare;
    public int criticalChanceMaxRare;
    public int criticalChanceMinEpic;
    public int criticalChanceMaxEpic;
    public int criticalChanceMinLegendary;
    public int criticalChanceMaxLegendary;

    public void NewAsset(EquipmentBase basic)
    {
        base.NewAsset(basic);
        damageMinCommon = basic.damageMinCommon;
        damageMaxCommon = basic.damageMaxCommon;
        damageMinRare = basic.damageMinRare;
        damageMaxRare = basic.damageMaxRare;
        damageMinEpic = basic.damageMinEpic;
        damageMaxEpic = basic.damageMaxEpic;
        damageMinLegendary = basic.damageMinLegendary;
        damageMaxLegendary = basic.damageMaxLegendary;

        defenseMinCommon = basic.defenseMinCommon;
        defenseMaxCommon = basic.defenseMaxCommon;
        defenseMinRare = basic.defenseMinRare;
        defenseMaxRare = basic.defenseMaxRare;
        defenseMinEpic = basic.defenseMinEpic;
        defenseMaxEpic = basic.defenseMaxEpic;
        defenseMinLegendary = basic.defenseMinLegendary;
        defenseMaxLegendary = basic.defenseMaxLegendary;

        strengthMinCommon = basic.strengthMinCommon;
        strengthMaxCommon = basic.strengthMaxCommon;
        strengthMinRare = basic.strengthMinRare;
        strengthMaxRare = basic.strengthMaxRare;
        strengthMinEpic = basic.strengthMinEpic;
        strengthMaxEpic = basic.strengthMaxEpic;
        strengthMinLegendary = basic.strengthMinLegendary;
        strengthMaxLegendary = basic.strengthMaxLegendary;

        criticalDamageMinCommon = basic.criticalDamageMinCommon;
        criticalDamageMaxCommon = basic.criticalDamageMaxCommon;
        criticalDamageMinRare = basic.criticalDamageMinRare;
        criticalDamageMaxRare = basic.criticalDamageMaxRare;
        criticalDamageMinEpic = basic.criticalDamageMinEpic;
        criticalDamageMaxEpic = basic.criticalDamageMaxEpic;
        criticalDamageMinLegendary = basic.criticalDamageMinLegendary;
        criticalDamageMaxLegendary = basic.criticalDamageMaxLegendary;

        vitalityMinCommon = basic.vitalityMinCommon;
        vitalityMaxCommon = basic.vitalityMaxCommon;
        vitalityMinRare = basic.vitalityMinRare;
        vitalityMaxRare = basic.vitalityMaxRare;
        vitalityMinEpic = basic.vitalityMinEpic;
        vitalityMaxEpic = basic.vitalityMaxEpic;
        vitalityMinLegendary = basic.vitalityMinLegendary;
        vitalityMaxLegendary = basic.vitalityMaxLegendary;

        criticalChanceMinCommon = basic.criticalChanceMinCommon;
        criticalChanceMaxCommon = basic.criticalChanceMaxCommon;
        criticalChanceMinRare = basic.criticalChanceMinRare;
        criticalChanceMaxRare = basic.criticalChanceMaxRare;
        criticalChanceMinEpic = basic.criticalChanceMinEpic;
        criticalChanceMaxEpic = basic.criticalChanceMaxEpic;
        criticalChanceMinLegendary = basic.criticalChanceMinLegendary;
        criticalChanceMaxLegendary = basic.criticalChanceMaxLegendary;
    }
}