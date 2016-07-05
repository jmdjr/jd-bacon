using UnityEngine;
using System;
using System.Collections;

using Object = UnityEngine.Object;
using System.Collections.Generic;

// When objects need to die, or be removed from the game, call on the __LevelMasterObject's Grim Reaper to take care of them.
public class GrimReaper : MonoBehaviour
{
    private static Queue<GameObject> corpses = new Queue<GameObject>();

    public void Kill(GameObject corpse)
    {
        if (corpse != null)
        {
            corpse.GetComponent<Renderer>().enabled = false;
            corpse.GetComponent<Collider>().enabled = false;

            corpses.Enqueue(corpse);
        }
    }

    private IEnumerator Reaping()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            while (corpses.Count > 0)
            { 
                yield return new WaitForEndOfFrame();
                var corpse = corpses.Dequeue();
                if (corpse != null)
                {
                    Destroy(corpse);
                }
            }
            yield return new WaitForSeconds(5);
        }
    }

    public void Awake()
    {
        this.StartCoroutine(Reaping());
    }
}
