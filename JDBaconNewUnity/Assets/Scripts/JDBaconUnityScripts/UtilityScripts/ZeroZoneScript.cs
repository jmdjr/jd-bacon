using UnityEngine;
using System;
using System.Collections;
using System.Linq.Expressions;

using Object = UnityEngine.Object;
using System.Collections.Generic;

public class ZeroZoneScript : MonoBehaviour 
{
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.rigidbody.useGravity = false;
            other.gameObject.renderer.enabled = false;
            other.gameObject.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            Debug.Log("hit zero zone");
        }
        else
        {
            Destroy(other);
        }

    }
}