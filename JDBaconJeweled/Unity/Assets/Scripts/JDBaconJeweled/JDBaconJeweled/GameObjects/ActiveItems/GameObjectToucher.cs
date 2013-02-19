using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;
using System.Collections.Generic;


public class GameObjectToucher: JDMonoGuiBehavior
{
    static GameObjectToucher instance;
    public static GameObjectToucher Instance
    {
        get
        {
            if (instance == null)
            {
                if (JDGame.GameMaster != null)
                {
                    instance = JDGame.GameMaster.GetComponent<GameObjectToucher>();
                }
            }

            return instance;
        }
    }

    private Stack<GameObject> pickedUpObjects = new Stack<GameObject>();
    private Stack<GameObject> droppedObjects = new Stack<GameObject>();

    public event GameObjectTransferEvent PickUpGameObject;
    public event GameObjectTransferEvent DropGameObject;

    public override void Update()
    {
        base.Update();
        
        if (Input.GetMouseButtonDown(0))
        {
            PickUpGameObjectAction();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            DropGameObjectAction();
        }
    }

    public GameObject LastPickedUpGameObject
    {
        get
        {
            if (this.pickedUpObjects != null && this.pickedUpObjects.Count > 0)
            {
                return this.pickedUpObjects.Peek();
            }
            else
                return null;
        }
    }
    public GameObject LastDroppedGameObject
    {
        get
        {
            if (this.droppedObjects != null && this.droppedObjects.Count > 0)
            {
                return this.droppedObjects.Peek();
            }
            else
                return null;
        }
    }

    public void ClearPickedUpList()
    {
        pickedUpObjects.Clear();
    }
    public void ClearDroppedList()
    {
        droppedObjects.Clear();
    }
    public void ClearHistory()
    {
        ClearDroppedList();
        ClearPickedUpList();
    }

    private void PickUpGameObjectAction()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction, Color.cyan, 50);

        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.red, hit.distance);
            //Debug.Log(hit.transform.gameObject.name);
            var go = hit.transform.gameObject;

            if (PickUpGameObject != null)
            {
                PickUpGameObject(new GameObjectTransferEventArgs(go, new Position2D((int)hit.point.x, (int)hit.point.y)));
            }

            storePickedUpGO(go);
        }
    }
    private void DropGameObjectAction()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction, Color.cyan, 50);

        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.red, hit.distance);
            //Debug.Log(hit.transform.gameObject.name);
            var go = hit.transform.gameObject;

            if (DropGameObject != null)
            {
                DropGameObject(new GameObjectTransferEventArgs(go, new Position2D((int)hit.point.x, (int)hit.point.y)));
            }

            storeDroppedGO(go);
        }
    }

    private void storePickedUpGO(GameObject go)
    {
        pickedUpObjects.Push(go);
    }
    private void storeDroppedGO(GameObject go)
    {
        droppedObjects.Push(go);
    }
}