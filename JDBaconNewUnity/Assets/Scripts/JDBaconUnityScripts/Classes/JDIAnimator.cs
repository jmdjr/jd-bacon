
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using SmoothMoves;

using Object = UnityEngine.Object;

public interface JDIAnimator
{
    Enum AnimationType { get; set; }
    BoneAnimation Bone { get; set; }

    void PlayAnimation(Enum type);
    void PlayCurrentAnimation();
    bool IsCurrentAnimationComplete();
}