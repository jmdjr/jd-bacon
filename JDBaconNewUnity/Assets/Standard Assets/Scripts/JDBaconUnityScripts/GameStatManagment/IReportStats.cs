using UnityEngine;
using System.Collections;

public class IReportStats : MonoBehaviour
{
    public Object StatManager;

    public void Start()
    {
        GameObject statManager = StatManager as GameObject;

        if (statManager != null && statManager.GetComponent(typeof(_GameSaveLoad)) != null)
        {
            UserData currentStats = ((_GameSaveLoad)statManager.GetComponent(typeof(_GameSaveLoad))).myData;
            currentStats._iUser.PlayerName = "Johnny";
            Debug.Log(((_GameSaveLoad)statManager.GetComponent(typeof(_GameSaveLoad))).myData._iUser.PlayerName);
        }
    }
}
