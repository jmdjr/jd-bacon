using UnityEngine;
using System;
using System.Collections;
using SmoothMoves;

public class HeroAttackingSM : JDStateMachine
{
    private HeroPhysicsProperties PhysicsProperties;
    private HeroAnimationProperties AnimationProperties;
    private JDMonoBodyBehavior ScriptReference;

    public HeroAttackingSM(JDHeroCharacter HeroReference, JDMonoBodyBehavior scriptReference)
    :base("Attacking")
    {
        this.PhysicsProperties = HeroReference.PhysicsProperties;
        this.AnimationProperties = HeroReference.AnimationProperties;
        this.ScriptReference = scriptReference;
    }

    #region States
    State IdleCombatState = new State("No Combat");
    State AttackingState = new State("Attacking");
    State DamageState = new State("Taking Damage");
    State SwitchingWeaponsState = new State("Switching between Weapons");
    #endregion

    #region Actions
    private IEnumerator SwitchingWeaponsAction()
    {
        if (!PhysicsProperties.IsAirborne)
        {
            AnimationProperties.UpdateStandardAnimation(HeroAnimationType.S_STAND);
        }
        yield return 0;
    }
    private IEnumerator IdleCombatAction()
    {
        yield return 0;
    }
    private IEnumerator AttackingAction()
    {
        yield return 0;
    }
    private IEnumerator DamageAction()
    {
        yield return 0;
    }
    #endregion

    #region Conditions
    private bool SwitchingWeaponsCondition()
    {
        // You have pressed the button to attack.
        return Input.GetAxis("Fire2") > 0;
    }
    private bool IdleCombatCondition()
    {
        return true;
    }
    private bool AttackingCondition()
    {
        // You have pressed the button to attack.
        return Input.GetAxis("Fire1") > 0;
    }
    private bool DamageCondition()
    {
        // You have been hit by an enemy.
        return true;
    }
    #endregion

    public override void  InitializeStateMachine()
    {
        ExitStateCondition ToSwitchingWeapons = new ExitStateCondition(SwitchingWeaponsCondition, SwitchingWeaponsState);
        ExitStateCondition ToIdleCombatState = new ExitStateCondition(IdleCombatCondition, IdleCombatState);
        ExitStateCondition ToAttackingState = new ExitStateCondition(AttackingCondition, AttackingState);

        SwitchingWeaponsState.Action = SwitchingWeaponsAction;
        SwitchingWeaponsState.AddExitCondition(ToIdleCombatState);
        SwitchingWeaponsState.AddExitCondition(ToAttackingState);

        IdleCombatState.Action = IdleCombatAction;
        IdleCombatState.AddExitCondition(ToSwitchingWeapons);
        IdleCombatState.AddExitCondition(ToAttackingState);

        AttackingState.Action = AttackingAction;
        AttackingState.AddExitCondition(ToIdleCombatState);
        AttackingState.AddExitCondition(ToAttackingState);

        this.SetInitialState(IdleCombatState);
    }

}
