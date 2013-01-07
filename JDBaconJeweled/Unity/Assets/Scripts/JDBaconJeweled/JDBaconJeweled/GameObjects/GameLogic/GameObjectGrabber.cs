using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;
using System.Collections.Generic;

public class GameObjectGrabber : JDMonoGuiBehavior
{
    public event GameObjectTransferEvent GrabbedGameObject;
    public event GameObjectTransferEvent DroppedGameObject;
    private GameObject heldGameObject;
     
    public override void Awake()
    {
        base.Awake();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Debug.DrawRay(ray.origin, ray.direction * 100, Color.green, 5);

            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                Debug.Log(hit.transform.gameObject.name);

                //Debug.Log(hit.point);
                if (GrabbedGameObject != null)
                {
                    GrabbedGameObject(new GameObjectTransferEventArgs(hit.transform.gameObject, new Position2D((int)hit.point.x, (int)hit.point.y)));
                    this.heldGameObject = hit.transform.gameObject;
                    this.heldGameObject.layer = 2;

                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Debug.DrawRay(ray.origin, ray.direction * 100, Color.green, 5);

            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                //Debug.Log(hit.transform.gameObject.name);

                //Debug.Log(hit.point);
                if (DroppedGameObject != null)
                {
                    DroppedGameObject(new GameObjectTransferEventArgs(hit.transform.gameObject, new Position2D((int)hit.point.x, (int)hit.point.y)));
                }
            }
        }
    }

}
