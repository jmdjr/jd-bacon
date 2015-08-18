using UnityEngine;
using System.Collections;

public class NodeGizmo : MonoBehaviour {
    // particle_1.png
    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "particle_1.png");
    }
}
