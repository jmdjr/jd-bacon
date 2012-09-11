using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmoothMoves;
using UnityEngine;

public enum CharacterAnimationType
{
    NONE,
    WALK,
    STAND,
    JUMP,
    ATTACK
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
            case CharacterAnimationType.ATTACK:
                return "Attack";
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

    public void PlayAnimation(CharacterAnimationType type, PlayMode mode = PlayMode.StopAll)
    {
        this.CurrentAnimation = type;
        this.PlayCurrentAnimation(mode);
    }

    public void PlayCurrentAnimation( PlayMode mode = PlayMode.StopAll)
    {
        Bone.Play(this.CurrentAnimation.TypeToString(), mode);
    }

    public bool CurrentAnimationCompleted()
    {
        return Bone.isPlaying;
    }
}