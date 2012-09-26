
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using SmoothMoves;

using Object = UnityEngine.Object;

public interface JDIAnimator
{
    BoneAnimation Bone { get; set; }

    void PlayCurrentAnimation();
    bool IsCurrentAnimationComplete();
}