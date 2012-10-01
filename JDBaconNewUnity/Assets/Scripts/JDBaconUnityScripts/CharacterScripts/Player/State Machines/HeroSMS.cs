using UnityEngine;
using System;
using System.Collections;
using SmoothMoves;

public class HeroSMS : JDStateMachineSystem
{
    #region Variables
    public JDHeroCharacter HeroReference;

    public HeroHorizontalMovementSM HeroHorizontalMovementSM;
    public HeroJumpingSM HeroJumpingSM;
    public HeroAttackingSM HeroAttackingSM;
    #endregion

    public HeroSMS(JDMonoBodyBehavior scriptReference, JDHeroCharacter heroProps)
        : base(scriptReference)
    {
        HeroReference = heroProps;
    }

    protected override void InitializeStateManager()
    {
        base.InitializeStateManager();

        this.HeroHorizontalMovementSM = new HeroHorizontalMovementSM(this.HeroReference, (JDMonoBodyBehavior)this.ScriptReference);
        this.HeroJumpingSM = new HeroJumpingSM(HeroReference, (JDMonoBodyBehavior)this.ScriptReference);
        this.HeroAttackingSM = new HeroAttackingSM(HeroReference, (JDMonoBodyBehavior)this.ScriptReference);

        this.MachineList.Add(this.HeroHorizontalMovementSM);
        this.MachineList.Add(this.HeroJumpingSM);
        this.MachineList.Add(this.HeroAttackingSM);
    }
}
