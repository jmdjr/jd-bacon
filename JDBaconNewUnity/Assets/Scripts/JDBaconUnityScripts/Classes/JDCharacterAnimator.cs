using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmoothMoves;
using UnityEngine;

public class JDHeroAnimator : JDIAnimator
{
    private HeroAnimationType CurrentAnimation;

    public Enum AnimationType
    {
        get
        {
            return this.CurrentAnimation;
        }
        set
        {
            this.CurrentAnimation = (HeroAnimationType)value;
        }
    }

    public BoneAnimation Bone
    {
        get
        {
            throw new NotImplementedException();
        }
        set
        {
            throw new NotImplementedException();
        }
    }

    public void PlayAnimation(Enum type)
    {
        throw new NotImplementedException();
    }

    public void PlayCurrentAnimation()
    {
        throw new NotImplementedException();
    }

    public bool IsCurrentAnimationComplete()
    {
        throw new NotImplementedException();
    }
}