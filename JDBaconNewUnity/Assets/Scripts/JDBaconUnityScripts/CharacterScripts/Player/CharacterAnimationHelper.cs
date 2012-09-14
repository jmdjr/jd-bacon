using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmoothMoves;
using UnityEngine;

// Character animations will be stored as a set of 3 flagged bit ranges.  each range represents a collection of unique animations
//
// | Standard Animations | Directions |   Weapons   |
// |   0000      00      |   00  00   |  00   0000  |
//
// This allows for a maximum of 32 unique animations, for 8 directions, utilizing 32 different weapons.
//  if these quantities need to be extended, a double

public enum CharacterAnimationType : ushort
{
    // Standard Animations
    NONE            = 0,
    STAND           = 1,
    WALK            = 2,
    JUMP            = 3,
    WALK_SWING      = 4,
    WALK_SHOOT      = 5,

    STANDARD_MASK   = 63,
    
    // Directions
    STRAIT          = 1 << 6,
    DIAGONAL        = 2 << 6,
    UP              = 3 << 6,
    DOWN,

    DIRECTION_MASK  = 15 << 6,

    // Weapons
    SWORD           = 1 << 10,
    SHOTGUN         = 2 << 10,
    GRENADE         = 3 << 10,
    
    WEAPON_MASK     = 63 << 10,
}
public static class CharacterAnimationTypeExtension
{
    public static string TypeToString(this CharacterAnimationType type)
    {
        switch (type)
        {
            case CharacterAnimationType.WALK:
                return "Walk";
            case CharacterAnimationType.JUMP:
                return "Jump";
            case CharacterAnimationType.STAND:
                return "Stand";
            default:
            case CharacterAnimationType.NONE:
                return "None";
                
        }
    }
}

public class CharacterAnimationHelper
{
    private CharacterAnimationType CurrentAnimation;
    //private 
    private BoneAnimation Bone;
    public CharacterAnimationType CurrentAnimationType { get { return CurrentAnimation; } }
    public CharacterAnimationHelper(BoneAnimation bone, CharacterAnimationType initialAnimation = CharacterAnimationType.STAND)
    {
        if (bone == null)
        {
            throw new Exception("Bone Cannot be null");
        }

        this.Bone = bone;
        this.CurrentAnimation = initialAnimation;
        this.PlayCurrentAnimation();
    }

    public void PlayAnimation(CharacterAnimationType type, PlayMode mode = PlayMode.StopSameLayer)
    {
        this.CurrentAnimation = type;
        this.PlayCurrentAnimation(mode);
    }

    public void PlayCurrentAnimation(PlayMode mode = PlayMode.StopSameLayer)
    {
        Bone.Play(this.CurrentAnimation.TypeToString(), mode);
    }

    public bool CurrentAnimationCompleted()
    {
        return !Bone.isPlaying;
    }
}