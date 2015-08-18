using UnityEngine;
using System;

public class ApplyExternalForces : MonoBehaviour
{
    public Vector3 ForcesToApply;

    public void OnTriggerStay(Collider collider)
    {
        if (collider.collider.rigidbody != null)
        {
            collider.collider.rigidbody.velocity += ForcesToApply;
            Debug.Log("Velocity of Collider :" + collider.collider.rigidbody.velocity.ToString());
        }
    }
}
