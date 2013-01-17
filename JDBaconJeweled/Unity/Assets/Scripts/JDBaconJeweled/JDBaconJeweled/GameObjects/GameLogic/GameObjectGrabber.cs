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
    public SwapTypes SwapType = SwapTypes.DRAG_DROP;

    static GameObjectGrabber instance;
    public static GameObjectGrabber Instance
    {
        get
        {
            if (instance == null)
            {
                if (JDGame.GameMaster != null)
                {
                    instance = JDGame.GameMaster.GetComponent<GameObjectGrabber>();
                }
            }

            return instance;
        }
    }

    public GameObject HeldGameObject { get { return this.heldGameObject; } }

    public override void Awake()
    {
        base.Awake();
    }

    public override void Update()
    {
        base.Update();
        switch (SwapType)
        {
            case SwapTypes.CLICK:
                ClickSwap();
                break;

            case SwapTypes.DRAG_DROP:
                DragDropSwap();
                break;
        }

        if (this.heldGameObject != null)
        {

        }
    }

    private void DragDropSwap()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction, Color.cyan, 50);

            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                if (GrabbedGameObject != null)
                {
                    GrabbedGameObject(new GameObjectTransferEventArgs(hit.transform.gameObject, new Position2D((int)hit.point.x, (int)hit.point.y)));
                }

                if (this.heldGameObject != null && this.heldGameObject.layer != 1)
                {
                    this.heldGameObject.layer = 1;
                    this.heldGameObject = null;
                }

                Debug.Log(hit.distance);

                this.heldGameObject = hit.transform.gameObject;
                this.heldGameObject.layer = 2;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction, Color.cyan, 50);
            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                if (DroppedGameObject != null)
                {
                    DroppedGameObject(new GameObjectTransferEventArgs(hit.transform.gameObject, new Position2D((int)hit.point.x, (int)hit.point.y)));
                }

                if (this.heldGameObject != null)
                {
                    this.heldGameObject.layer = 1;
                    this.heldGameObject = null;
                }
                Debug.Log(this.heldGameObject.GetComponent<FallingBullet>().Name);
            }
        }
    }

    private void ClickSwap()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                Debug.Log(hit.transform.gameObject.name);

                if (this.heldGameObject == null)
                {
                    if (GrabbedGameObject != null)
                    {
                        GrabbedGameObject(new GameObjectTransferEventArgs(hit.transform.gameObject, new Position2D((int)hit.point.x, (int)hit.point.y)));
                    }
                    if (this.heldGameObject != null && this.heldGameObject.layer != 1)
                    {
                        this.heldGameObject.layer = 1;
                        this.heldGameObject = null;
                    }
                    
                    this.heldGameObject = hit.transform.gameObject;
                    this.heldGameObject.layer = 2;
                }
                else if (this.heldGameObject != null)
                {
                    if (DroppedGameObject != null)
                    {
                        DroppedGameObject(new GameObjectTransferEventArgs(hit.transform.gameObject, new Position2D((int)hit.point.x, (int)hit.point.y)));
                    }

                    if (this.heldGameObject != null)
                    {
                        this.heldGameObject.layer = 1;
                        this.heldGameObject = null;
                    }
                }
            }
        }
    }
}
