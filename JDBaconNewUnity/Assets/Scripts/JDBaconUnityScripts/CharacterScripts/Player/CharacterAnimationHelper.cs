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

public enum AnimationType
{
    // Standard Animations
    S_NONE              = 0,
    S_STAND             = 1,
    S_WALK              = 2,
    S_JUMP              = 3,
    S_STAND_SWING       = 4,
    S_STAND_SHOOT       = 5,
    S_WALK_SWING        = 6,
    S_WALK_SHOOT        = 7,

    MASK_STANDARD       = 4095,
    
    // Directions
    D_STRAIT            = 1 << 12,
    D_DIAGONAL_UP       = 2 << 12,
    D_UP                = 3 << 12,
    D_DIAGONAL_DOWN     = 4 << 12,
    D_DOWN              = 5 << 12,

    MASK_DIRECTION      = 255 << 12,

    // Weapons
    W_NONE              = 1 << 20,
    W_SWORD             = 2 << 20,
    W_SHOTGUN           = 3 << 20,
    W_GRENADE           = 4 << 20,
    
    MASK_WEAPON         = 4095 << 20,
}
public static class CharacterAnimationTypeExtension
{
    public static string TypeToDirectionalString(this AnimationType type)
    {
        switch (type.TypeToDirection())
        {
            default:
            case AnimationType.D_STRAIT:
                return "FaceStrait";

            case AnimationType.D_DOWN:
                return "FaceDown";

            case AnimationType.D_UP:
                return "FaceUp";

            case AnimationType.D_DIAGONAL_UP:
                return "FaceDiagonalUp";

            case AnimationType.D_DIAGONAL_DOWN:
                return "FaceDiagonalDown";
        }
    }
    public static string TypeToWeaponString(this AnimationType type)
    {
        switch (type.TypeToWeapon())
        {
            default:
            case AnimationType.W_NONE:
                return "NoWeapon";

            case AnimationType.W_SWORD:
                return "Sword";

            case AnimationType.W_SHOTGUN:
                return "Shotgun";
        }
    }
    public static string TypeToStandardString(this AnimationType type)
    {
        switch (type.TypeToStandard())
        {
            default:
            case AnimationType.S_STAND:
                return "Stand";

            case AnimationType.S_WALK_SWING:
                return "WalkSwing";

            case AnimationType.S_WALK:
                return "Walk";

            case AnimationType.S_JUMP:
                return "Jump";
        }
    }

    // Parse Animation Type cast into proper name for animation.
    public static string TypeToAnimationString(this AnimationType type)
    {
        return type.TypeToDirectionalString() + type.TypeToWeaponString() + type.TypeToStandardString();
    }

    public static AnimationType TypeToStandard(this AnimationType type)
    {
        return (AnimationType)(AnimationType.MASK_STANDARD & type);
    }
    
    public static AnimationType TypeToWeapon(this AnimationType type)
    {
        return (AnimationType)(AnimationType.MASK_WEAPON & type);
    }

    public static AnimationType TypeToDirection(this AnimationType type)
    {
        return (AnimationType)(AnimationType.MASK_DIRECTION & type);
    }
}

public class CharacterAnimationHelper
{
    private AnimationType CurrentAnimation;
    //private 
    private BoneAnimation Bone;
    public AnimationType CurrentAnimationType { get { return CurrentAnimation; } }

    public AnimationType CurrentStandardAnimation { get { return CurrentAnimation.TypeToStandard(); } }
    public AnimationType CurrentWeaponAnimation { get { return CurrentAnimation.TypeToWeapon(); } }
    public AnimationType CurrentDirectionalAnimation { get { return CurrentAnimation.TypeToDirection(); } }

    public CharacterAnimationHelper(BoneAnimation bone, AnimationType initialAnimation = AnimationType.S_STAND | AnimationType.W_NONE | AnimationType.D_STRAIT)
    {
        if (bone == null)
        {
            throw new Exception("Bone Cannot be null");
        }

        this.Bone = bone;
        this.CurrentAnimation = initialAnimation;
        this.PlayCurrentAnimation();
    }

    public void PlayAnimation(AnimationType type)
    {
        this.CurrentAnimation = type;
        this.PlayCurrentAnimation();
    }

    public void PlayCurrentAnimation()
    {
        Debug.Log(this.CurrentAnimation.TypeToStandardString());
        Bone.Play(this.CurrentAnimation.TypeToStandardString(), PlayMode.StopSameLayer);

        if (this.CurrentWeaponAnimation != AnimationType.W_NONE)
        {
            Bone.Play(this.CurrentAnimation.TypeToWeaponString(), PlayMode.StopSameLayer);
        }
    }

    public bool CurrentAnimationCompleted()
    {
        return !Bone.isPlaying;
    }
}