using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmoothMoves;
using UnityEngine;

public class JDHeroAnimator : JDIAnimator
{
    private HeroAnimationType currentAnimation;

    private Enum AnimationType
    {
        get
        {
            return this.currentAnimation;
        }
        set
        {
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

    public void PlayCurrentAnimation()
    {
        Debug.Log(this.currentAnimation.TypeToStandardString());
        Bone.Play(this.currentAnimation.TypeToStandardString(), PlayMode.StopSameLayer);

        if (this.CurrentWeaponAnimation != HeroAnimationType.W_NONE)
        {
            Bone.Play(this.currentAnimation.TypeToWeaponString(), PlayMode.StopSameLayer);
        }
    }

    public bool IsCurrentAnimationComplete()
    {
        return !Bone.isPlaying;
    }

    public void PlayAnimation(HeroAnimationType heroAnimationType)
    {
        this.AnimationType = heroAnimationType;
        this.PlayCurrentAnimation();
    }
}