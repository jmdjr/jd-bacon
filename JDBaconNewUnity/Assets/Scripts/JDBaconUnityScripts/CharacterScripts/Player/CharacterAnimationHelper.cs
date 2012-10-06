using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmoothMoves;
using UnityEngine;

// Character animations will be stored as a set of 3 flagged bit ranges.  each range represents a collection of unique animations
//
// | Standard Animations |   Directions   |      Weapons     |
// |   0000 0000  0000   |   0000  0000   |  0000 0000 0000  |
//
// This allows for a maximum of ~62 unique animations, for ~14 directions, utilizing ~62 different weapons.
//  if these quantities need to be extended, a 32 bit int will be used instead, in which each of the three flag regions will be
//  scaled appropriately.

[Serializable]
public class CharacterAnimationHelper : JDIAnimator
{
    [SerializeField]
    private BoneAnimation bone;
    public BoneAnimation Bone
    {
        get
        {
            return this.bone;
        }
        set
        {
            this.bone = value;
        }
    }

    private HeroAnimationType CurrentAnimation;
    public HeroAnimationType CurrentAnimationType { get { return CurrentAnimation; } }

    public HeroAnimationType CurrentStandardAnimation { get { return CurrentAnimation.TypeToStandard(); } }
    public HeroAnimationType CurrentWeaponAnimation { get { return CurrentAnimation.TypeToWeapon(); } }
    public HeroAnimationType CurrentDirectionalAnimation { get { return CurrentAnimation.TypeToDirection(); } }

    public CharacterAnimationHelper(BoneAnimation bone, HeroAnimationType initialAnimation = HeroAnimationType.S_STAND | HeroAnimationType.W_NONE | HeroAnimationType.D_STRAIT)
    {
        if (bone == null)
        {
            throw new Exception("Bone Cannot be null");
        }

        this.Bone = bone;
        this.CurrentAnimation = initialAnimation;
        this.PlayCurrentAnimation();
    }

    public void PlayAnimation(HeroAnimationType type)
    {
        this.CurrentAnimation = type;
        this.PlayCurrentAnimation();
    }

    public void PlayCurrentAnimation()
    {
        Bone.Play(this.CurrentAnimation.TypeToStandardString(), PlayMode.StopSameLayer);
        Bone.Play(this.CurrentAnimation.TypeToWeaponString(), PlayMode.StopSameLayer);

        Debug.Log(this.CurrentStandardAnimation.TypeToStandardString() + " " + this.CurrentWeaponAnimation.TypeToWeaponString());
    }

    public bool IsCurrentAnimationComplete()
    {
        return !Bone.isPlaying;
    }
}