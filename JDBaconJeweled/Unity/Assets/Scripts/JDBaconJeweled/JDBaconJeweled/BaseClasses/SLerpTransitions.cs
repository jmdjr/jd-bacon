using UnityEngine;
using System;
using System.Collections;
using System.Linq.Expressions;

using Object = UnityEngine.Object;
using System.Collections.Generic;

public static class SLerpTransitions
{
    public static IEnumerator MoveWithConstantTimeTo(this Transform transform, Transform positionOrientation, float TransitionDuration)
    {
        float t = 0.0f;
        Vector3 startingPos = transform.position;
        Quaternion startingOri = transform.rotation;

        Vector3 position = positionOrientation.position;
        Quaternion orientation = positionOrientation.rotation;

        if (position != Vector3.zero || orientation != Quaternion.identity)
        {
            while (t < 1.0f)
            {
                t += Time.deltaTime * (Time.timeScale / TransitionDuration);

                if (position != Vector3.zero)
                {
                    transform.position = Vector3.Lerp(startingPos, position, t);
                }

                if (orientation != Quaternion.identity)
                {
                    transform.rotation = Quaternion.Slerp(startingOri, orientation, t);
                }

                yield return 0;
            }
        }
    }
    public static IEnumerator MoveWithConstantTimeTo(this Transform transform, Vector3 position, Quaternion orientation, float TransitionDuration)
    {
        float t = 0.0f;
        Vector3 startingPos = transform.position;
        Quaternion startingOri = transform.rotation;

        if (position != Vector3.zero || orientation != Quaternion.identity)
        {
            while (t < 1.0f)
            {
                t += Time.deltaTime * (Time.timeScale / TransitionDuration);

                if (position != Vector3.zero)
                {
                    transform.position = Vector3.Lerp(startingPos, position, t);
                }

                if (orientation != Quaternion.identity)
                {
                    transform.rotation = Quaternion.Slerp(startingOri, orientation, t);
                }

                yield return 0;
            }
        }
    }
    public static IEnumerator MoveWithConstantSpeedTo(this Transform transform, Transform positionOrientation, float speed)
    {
        Vector3 startingPos = transform.position;
        Quaternion startingOri = transform.rotation;


        if (positionOrientation.position != Vector3.zero || positionOrientation.rotation != Quaternion.identity)
        {
            float i = 0.0f;
            while (i < 1.0f)
            {
                Vector3 endingPos = positionOrientation.position;
                Quaternion endingOri = positionOrientation.rotation;
                float distance = Vector3.Distance(startingPos, endingPos);
                i += (speed * Time.deltaTime) / distance;

                if (endingPos != Vector3.zero)
                {
                    transform.position = Vector3.Lerp(startingPos, endingPos, i);
                }

                if (endingOri != Quaternion.identity)
                {
                    transform.rotation = Quaternion.Slerp(startingOri, endingOri, i);
                }

                yield return 0;
            }
        }
    }
    public static IEnumerator MoveWithConstantSpeedTo(this Transform transform, Vector3 endingPos, Quaternion endingOri, float speed)
    {
        Vector3 startingPos = transform.position;
        Quaternion startingOri = transform.rotation;

        if (endingPos != Vector3.zero || endingOri != Quaternion.identity)
        {
            float i = 0.0f;
            while (i < 1.0f)
            {
                float distance = Vector3.Distance(startingPos, endingPos);
                i += (speed * Time.deltaTime) / distance;

                if (endingPos != Vector3.zero)
                {
                    transform.position = Vector3.Lerp(startingPos, endingPos, i);
                }

                if (endingOri != Quaternion.identity)
                {
                    transform.rotation = Quaternion.Slerp(startingOri, endingOri, i);
                }

                yield return 0;
            }
        }
    }
}
