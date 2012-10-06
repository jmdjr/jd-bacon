using UnityEngine;
using System;
using System.Collections;
using SmoothMoves;

public class HeroAttackingSM : JDStateMachine
{
    private JDHeroCharacter heroReference;
    private HeroPhysicsProperties PhysicsProperties;
    private HeroAnimationProperties AnimationProperties;
    private JDMonoBodyBehavior ScriptReference;
    private JDWeaponManager WeaponManager;
    private bool takenAHit = false;

    public HeroAttackingSM(JDHeroCharacter HeroReference, JDMonoBodyBehavior scriptReference)
    :base("Attacking")
    {
        this.heroReference = HeroReference;
        this.PhysicsProperties = HeroReference.PhysicsProperties;
        this.AnimationProperties = HeroReference.AnimationProperties;
        this.ScriptReference = scriptReference;
        this.WeaponManager = heroReference.WeaponManager;

        this.ScriptReference.ScriptCollisionEnter += new MonoBodyScriptEventHanlder(ScriptReference_ScriptCollisionEnter);
    }

    private void ScriptReference_ScriptCollisionEnter(CollisionEventArgs eventArgs)
    {

    }

    #region Idle
    State IdleCombatState;
    private IEnumerator IdleCombatAction()
    {
        this.AnimationProperties.UpdateWeaponAnimation(this.WeaponManager.GetWeaponIdle());
        yield return 0;
    }
    private bool IdleCombatCondition()
    {

        return Input.GetAxis("Fire1") == 0;
    }
    #endregion

    #region Attacking
    State AttackingState;
    private IEnumerator AttackingAction()
    {
        this.AnimationProperties.UpdateWeaponAnimation(this.WeaponManager.GetWeaponAttack());
        yield return 0;
    }
    private bool AttackingCondition()
    {
        // You have pressed the button to attack.
        return Input.GetAxis("Fire1") > 0;
    }
    #endregion

    #region Taking Damage
    State DamageState;
    private IEnumerator DamageAction()
    {
        // maybe make hero invunerable for a second.
        yield return 0;
    }
    private bool DamageCondition()
    {
        // You have been hit by an enemy.
        return false;
    }
    #endregion

    #region Switching Weapon
    State SwitchingWeaponsState;
    private IEnumerator SwitchingWeaponsAction()
    {
        this.WeaponManager.GotoNextWeapon();
        yield return 0;
    }
    private bool SwitchingWeaponsCondition()
    {
        // You have pressed the button to attack.
        return Input.GetAxis("Fire3") > 0;
    }
    #endregion

    public override void  InitializeStateMachine()
    {
        IdleCombatState = new State("Idle, not attacking")
        {
            Action = IdleCombatAction,
            RepeatActionCount = 1
        };

        SwitchingWeaponsState = new State("Switching Between Weapons") 
        { 
            Action = SwitchingWeaponsAction,
            RepeatActionCount = 1,
        };

        AttackingState = new State("Attacking") 
        {
            Action = AttackingAction,
            RepeatActionCount = 1,
        };

        ExitStateCondition ToSwitchingWeapons = new ExitStateCondition(SwitchingWeaponsCondition, SwitchingWeaponsState);
        ExitStateCondition ToIdleCombatState = new ExitStateCondition(IdleCombatCondition, IdleCombatState);
        ExitStateCondition ToAttackingState = new ExitStateCondition(AttackingCondition, AttackingState);

        SwitchingWeaponsState.AddExitCondition(ToIdleCombatState);
        SwitchingWeaponsState.AddExitCondition(ToAttackingState);

        IdleCombatState.AddExitCondition(ToSwitchingWeapons);
        IdleCombatState.AddExitCondition(ToAttackingState);
        
        AttackingState.AddExitCondition(ToIdleCombatState);
        AttackingState.AddExitCondition(ToAttackingState);

        this.SetInitialState(IdleCombatState);
    }
}
