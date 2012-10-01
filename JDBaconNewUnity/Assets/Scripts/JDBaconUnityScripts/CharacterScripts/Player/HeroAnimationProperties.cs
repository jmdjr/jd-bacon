using UnityEngine;
using System;
using System.Collections;
using SmoothMoves;

[Serializable]
public class HeroAnimationProperties
{
    public JDHeroAnimator Animator;
    private HeroAnimationType currentStandard = HeroAnimationType.S_STAND;
    private HeroAnimationType currentWeapon = HeroAnimationType.W_NONE;
    private HeroAnimationType currentDirection = HeroAnimationType.D_STRAIT;
    private bool facingLeft = false;

    public void FaceLeft()
    {
        if (!facingLeft)
        {
            this.Animator.Bone.gameObject.transform.Rotate(Vector3.up, 180.0f);
            facingLeft = true;
        }
    }
    public void FaceRight()
    {
        if (facingLeft)
        {
            this.Animator.Bone.gameObject.transform.Rotate(Vector3.up, 180.0f);
            facingLeft = false;
        }
    }

    public HeroAnimationProperties(JDHeroAnimator animator)
    {
        this.Animator = animator;
    }

    public void UpdateWeaponAnimation(HeroAnimationType weaponType)
    {
        this.currentWeapon = weaponType.TypeToWeapon();
        this.UpdateAnimation();
    }
    public void UpdateStandardAnimation(HeroAnimationType standardType)
    {
        this.currentStandard = standardType.TypeToStandard();
        this.UpdateAnimation();
    }
    public void UpdateDirectionAnimation(HeroAnimationType directionType)
    {
        this.currentDirection = directionType.TypeToDirection();
        this.UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        this.Animator.PlayAnimation(this.currentStandard | this.currentWeapon | this.currentDirection);
    }
}