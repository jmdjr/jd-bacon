using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmoothMoves;
using UnityEngine;

public class JDHeroAnimator : JDIAnimator
{
    private HeroAnimationType currentAnimation;
    private bool dirtyAnimation = false;
    private Enum AnimationType
    {
        get
        {
            return this.currentAnimation;
        }
        set
        {
            if (this.currentAnimation != (HeroAnimationType)value)
            {
                dirtyAnimation = true;
            }
            this.currentAnimation = (HeroAnimationType)value;
        }
    }

    public HeroAnimationType CurrentStandardAnimation { get { return ((HeroAnimationType)AnimationType).TypeToStandard(); } }
    public HeroAnimationType CurrentWeaponAnimation { get { return ((HeroAnimationType)AnimationType).TypeToWeapon(); } }
    public HeroAnimationType CurrentDirectionalAnimation { get { return ((HeroAnimationType)AnimationType).TypeToDirection(); } }

    public JDHeroAnimator(BoneAnimation bone = null, HeroAnimationType initialAnimation = HeroAnimationType.S_STAND | HeroAnimationType.W_NONE | HeroAnimationType.D_STRAIT)
    {
        if (bone == null)
        {
            throw new Exception("Bone Cannot be null");
        }

        this.Bone = bone;
        this.currentAnimation = initialAnimation;
        this.PlayCurrentAnimation();
    }

    public BoneAnimation Bone { get; set; }

    public bool IsCurrentAnimationComplete()
    {
        return !Bone.isPlaying;
    }

    public void PlayAnimation(HeroAnimationType heroAnimationType)
    {
        this.AnimationType = heroAnimationType;
        this.PlayCurrentAnimation();
    }

    public float GetCurrentClipLength()
    {
        return Bone[this.currentAnimation.TypeToWeaponString()].clip.length;
    }

    public void PlayCurrentAnimation()
    {
        if (this.dirtyAnimation)
        {
            Bone.Play(this.currentAnimation.TypeToStandardString(), PlayMode.StopSameLayer);
            Bone.Play(this.currentAnimation.TypeToWeaponString(), PlayMode.StopSameLayer);
        }
    }

}