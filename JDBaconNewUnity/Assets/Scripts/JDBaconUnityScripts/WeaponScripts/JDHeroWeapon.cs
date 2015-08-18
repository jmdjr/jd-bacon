using UnityEngine;
using System;
using System.Collections;
using SmoothMoves;

[Serializable]
public class JDHeroWeapon : JDIWeapon
{
    [SerializeField]
    private int damageAmount;
    [SerializeField]
    private bool isActive;
    [SerializeField]
    private float cooldownTime;
    [SerializeField]
    private string name;
    [SerializeField]
    private float range;

    [SerializeField]
    private HeroAnimationType weaponIdleAnimationType;
    [SerializeField]
    private HeroAnimationType weaponAttackAnimationType;

    private HeroWeaponIconType weaponIconType;

    protected int WeaponIndex;
    protected JDHeroCharacter heroReference;

    public int DamageAmount { get { return this.damageAmount; } set { this.damageAmount = value; } }
    public bool IsActive { get { return this.isActive; } set { this.isActive = value; } }
    public float CooldownTime { get { return this.cooldownTime; } set { this.cooldownTime = value; } }
    public string Name { get { return this.name; } set { this.name = value; } }
    public float Range { get { return range; } set { range = value; } }

    public HeroAnimationType WeaponIdleAnimationType { get { return weaponIdleAnimationType; } set { this.weaponIdleAnimationType = value; } }
    public HeroAnimationType WeaponAttackAnimationType { get { return weaponAttackAnimationType; } set { this.weaponAttackAnimationType = value; } }
    public HeroWeaponIconType WeaponIconType { get { return weaponIconType; } set { weaponIconType = value; } }

    public JDIObjectTypes JDType { get { return JDIObjectTypes.WEAPON; } }

    public virtual void AttackUpdate()
    {
        this.heroReference.AnimationProperties.UpdateWeaponAnimation(this.WeaponAttackAnimationType);
    }

    public bool ReportStatistics(JDIStatTypes stat, int valueShift)
    {
        return true;
    }

}